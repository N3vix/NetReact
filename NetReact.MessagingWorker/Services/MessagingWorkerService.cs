using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using NetReact.MessageBroker;
using NetReact.MessageBroker.SharedModels;
using RabbitMQ.Client.Events;

namespace NetReact.MessagingWorker.Services;

public class MessagingWorkerService : BackgroundService
{
    private readonly IServiceScopeFactory _factory;
    private readonly IMessageBrokerConsumer _createMessageCommandConsumer;
    private readonly IMessageBrokerConsumer _editMessageCommandConsumer;
    private readonly IMessageBrokerConsumer _deleteMessageCommandConsumer;

    public MessagingWorkerService(
        IServiceScopeFactory factory,
        IOptionsSnapshot<MessageBrokerChannelConnectionConfig> options,
        IMessageBrokerConsumerFactory consumerFactory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(consumerFactory);

        _factory = factory;
        var messageCreateCommandConfig = options.Get("MessageCreateCommand");
        _createMessageCommandConsumer = consumerFactory.Build(messageCreateCommandConfig);
        var messageEditCommandConfig = options.Get("MessageEditCommand");
        _editMessageCommandConsumer = consumerFactory.Build(messageEditCommandConfig);
        var messageDeleteCommandConfig = options.Get("MessageDeleteCommand");
        _deleteMessageCommandConsumer = consumerFactory.Build(messageDeleteCommandConfig);
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _createMessageCommandConsumer.AddListener(WriteMessage);
        _editMessageCommandConsumer.AddListener(EditMessage);
        _deleteMessageCommandConsumer.AddListener(DeleteMessage);

        return Task.CompletedTask;
    }

    private void WriteMessage(object? @object, BasicDeliverEventArgs args)
    {
        using var scope = _factory.CreateScope();
        var messagesService = scope.ServiceProvider.GetRequiredService<IMessagesService>();
        var command = GetCommand<CreateChannelMessageCommand>(args);
        messagesService.Add(command.SenderId, command.ChannelId, command.Content, command.Image);
    }

    private void EditMessage(object? @object, BasicDeliverEventArgs args)
    {
        using var scope = _factory.CreateScope();
        var messagesService = scope.ServiceProvider.GetRequiredService<IMessagesService>();
        var command = GetCommand<EditChannelMessageCommand>(args);
        messagesService.Update(command.MessageId, command.NewContent);
    }

    private void DeleteMessage(object? @object, BasicDeliverEventArgs args)
    {
        using var scope = _factory.CreateScope();
        var messagesService = scope.ServiceProvider.GetRequiredService<IMessagesService>();
        var command = GetCommand<DeleteChannelMessageCommand>(args);
        messagesService.Delete(command.MessageId);
    }

    private T GetCommand<T>(BasicDeliverEventArgs args)
    {
        var body = args.Body.ToArray();
        var messageJson = Encoding.UTF8.GetString(body);
        return JsonSerializer.Deserialize<T>(messageJson)!;
    }

    public override void Dispose()
    {
        _createMessageCommandConsumer.RemoveListener(WriteMessage);
        _editMessageCommandConsumer.RemoveListener(EditMessage);
        base.Dispose();
    }
}