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
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _createMessageCommandConsumer.AddListener(WriteMessage);
        _editMessageCommandConsumer.AddListener(EditMessage);

        return Task.CompletedTask;
    }

    private void WriteMessage(object? @object, BasicDeliverEventArgs args)
    {
        using var scope = _factory.CreateScope();
        var messagesService = scope.ServiceProvider.GetRequiredService<IMessagesService>();
        var body = args.Body.ToArray();
        var messageJson = Encoding.UTF8.GetString(body);
        var message = JsonSerializer.Deserialize<CreateChannelMessageCommand>(messageJson)!;
        messagesService.Add(message.SenderId, message.ChannelId, message.Content, message.Image);
    }
    
    private void EditMessage(object? @object, BasicDeliverEventArgs args)
    {
        using var scope = _factory.CreateScope();
        var messagesService = scope.ServiceProvider.GetRequiredService<IMessagesService>();
        var body = args.Body.ToArray();
        var messageJson = Encoding.UTF8.GetString(body);
        var message = JsonSerializer.Deserialize<EditChannelMessageCommand>(messageJson)!;
        messagesService.Update(message.MessageId, message.NewContent);
    }

    public override void Dispose()
    {
        _createMessageCommandConsumer.RemoveListener(WriteMessage);
        _editMessageCommandConsumer.RemoveListener(EditMessage);
        base.Dispose();
    }
}