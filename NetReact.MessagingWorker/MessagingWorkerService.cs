using System.Text;
using System.Text.Json;
using NetReact.MessageBroker;
using NetReact.MessageBroker.SharedModels;
using NetReact.MessagingWorker.Services;
using RabbitMQ.Client.Events;

namespace NetReact.MessagingWorker;

public class MessagingWorkerService : BackgroundService
{
    private readonly MessageBrokerChannelConnectionConfig _channelConnectionConfig
        = new() { ExchangeKey = "testExchange", QueueKey = "testQueue", RoutingKey = "testRoute" };

    private readonly IServiceScopeFactory _factory;
    private readonly IMessageBrokerConsumer _consumer;

    public MessagingWorkerService(
        IServiceScopeFactory factory,
        IMessageBrokerConsumerFactory consumer)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(consumer);

        _factory = factory;
        _consumer = consumer.Build(_channelConnectionConfig);
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _consumer.AddListener(WriteMessage);

        return Task.CompletedTask;
    }

    private void WriteMessage(object? @object, BasicDeliverEventArgs args)
    {
        using var scope = _factory.CreateScope();
        var messagesGateway = scope.ServiceProvider.GetRequiredService<IMessagesService>();
        var body = args.Body.ToArray();
        var messageJson = Encoding.UTF8.GetString(body);
        var message = JsonSerializer.Deserialize<ChannelMessageCreated>(messageJson)!;
        messagesGateway.Add(message.SenderId, message.ChannelId, message.Content, message.Image);
    }

    public override void Dispose()
    {
        _consumer.RemoveListener(WriteMessage);
        base.Dispose();
    }
}