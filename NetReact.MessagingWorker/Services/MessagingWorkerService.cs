using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using NetReact.MessageBroker;
using NetReact.MessageBroker.SharedModels;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Trace;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NetReact.MessagingWorker.Services;

public class MessagingWorkerService : BackgroundService
{
    private readonly Tracer _tracer;
    private readonly IServiceScopeFactory _factory;
    private readonly IMessageBrokerConsumer _createMessageCommandConsumer;
    private readonly IMessageBrokerConsumer _editMessageCommandConsumer;
    private readonly IMessageBrokerConsumer _deleteMessageCommandConsumer;

    private readonly IMessageBrokerProducer _createdMessageEventProducer;
    private readonly IMessageBrokerProducer _editedMessageEventProducer;
    private readonly IMessageBrokerProducer _deletedMessageEventProducer;

    public MessagingWorkerService(
        Tracer tracer,
        IServiceScopeFactory factory,
        IOptionsSnapshot<MessageBrokerChannelConnectionConfig> options,
        IMessageBrokerConsumerFactory consumerFactory,
        IMessageBrokerProducerFactory producerFactory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(consumerFactory);
        ArgumentNullException.ThrowIfNull(producerFactory);

        _tracer = tracer;
        _factory = factory;

        var messageCreateCommandConfig = options.Get("MessageCreateCommand");
        _createMessageCommandConsumer = consumerFactory.Build(messageCreateCommandConfig);
        var messageEditCommandConfig = options.Get("MessageEditCommand");
        _editMessageCommandConsumer = consumerFactory.Build(messageEditCommandConfig);
        var messageDeleteCommandConfig = options.Get("MessageDeleteCommand");
        _deleteMessageCommandConsumer = consumerFactory.Build(messageDeleteCommandConfig);

        var messageCreatedCommandConfig = options.Get("MessageCreatedCommand");
        _createdMessageEventProducer = producerFactory.Build(messageCreatedCommandConfig);
        var messageEditedCommandConfig = options.Get("MessageEditedCommand");
        _editedMessageEventProducer = producerFactory.Build(messageEditedCommandConfig);
        var messageDeletedCommandConfig = options.Get("MessageDeletedCommand");
        _deletedMessageEventProducer = producerFactory.Build(messageDeletedCommandConfig);
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _createMessageCommandConsumer.AddListener(CreateMessage);
        _editMessageCommandConsumer.AddListener(EditMessage);
        _deleteMessageCommandConsumer.AddListener(DeleteMessage);

        return Task.CompletedTask;
    }

    private async void CreateMessage(object? @object, BasicDeliverEventArgs args)
    {
        var parentContext = Propagators.DefaultTextMapPropagator.Extract(
            default, 
            args.BasicProperties,
            ExtractTraceContextFromBasicProperties);
        Baggage.Current = parentContext.Baggage;
        using var _ = _tracer.StartSpan(nameof(CreateMessage), SpanKind.Consumer, new SpanContext(parentContext.ActivityContext));

        using var scope = _factory.CreateScope();
        var messagesService = scope.ServiceProvider.GetRequiredService<IMessagesService>();
        var command = GetCommand<CreateChannelMessageCommand>(args);
        var messageId = await messagesService.Add(command.SenderId, command.ChannelId, command.Content, command.Image);

        var messageCreatedCommand = new ChannelMessageCreated
        {
            ChannelId = command.ChannelId,
            MessageId = messageId
        };
        _createdMessageEventProducer.SendMessage(messageCreatedCommand);
    }

    private async void EditMessage(object? @object, BasicDeliverEventArgs args)
    {
        using var scope = _factory.CreateScope();
        var messagesService = scope.ServiceProvider.GetRequiredService<IMessagesService>();
        var command = GetCommand<EditChannelMessageCommand>(args);
        var isUpdated = await messagesService.Update(command.MessageId, command.NewContent);
        if (!isUpdated) return;

        var messageEditedCommand = new ChannelMessageEdited { MessageId = command.MessageId };
        _editedMessageEventProducer.SendMessage(messageEditedCommand);
    }

    private async void DeleteMessage(object? @object, BasicDeliverEventArgs args)
    {
        using var scope = _factory.CreateScope();
        var messagesService = scope.ServiceProvider.GetRequiredService<IMessagesService>();
        var command = GetCommand<DeleteChannelMessageCommand>(args);
        var isDeleted = await messagesService.Delete(command.MessageId);
        if (!isDeleted) return;

        var messageEditedCommand = new ChannelMessageDeleted { MessageId = command.MessageId };
        _deletedMessageEventProducer.SendMessage(messageEditedCommand);
    }

    private T GetCommand<T>(BasicDeliverEventArgs args)
    {
        var body = args.Body.ToArray();
        var messageJson = Encoding.UTF8.GetString(body);
        return JsonSerializer.Deserialize<T>(messageJson)!;
    }    
    
    private IEnumerable<string> ExtractTraceContextFromBasicProperties(IBasicProperties props, string key)
    {
        try
        {
            if (props.Headers.TryGetValue(key, out var value))
            {
                var bytes = value as byte[];
                return new[] { Encoding.UTF8.GetString(bytes) };
            }
        }
        catch (Exception ex)
        {
            // logger.LogError(ex, "Failed to extract trace context.");
        }

        return Enumerable.Empty<string>();
    }

    public override void Dispose()
    {
        _createMessageCommandConsumer.RemoveListener(CreateMessage);
        _editMessageCommandConsumer.RemoveListener(EditMessage);
        _deleteMessageCommandConsumer.RemoveListener(DeleteMessage);

        _createMessageCommandConsumer.Dispose();
        _editMessageCommandConsumer.Dispose();
        _deleteMessageCommandConsumer.Dispose();

        _createdMessageEventProducer.Dispose();
        _editedMessageEventProducer.Dispose();
        _deletedMessageEventProducer.Dispose();
        base.Dispose();
    }
}