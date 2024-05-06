using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;

namespace NetReact.MessageBroker;

public abstract class MessageBrokerConsumerBase : MessageBrokerBase, IMessageBrokerConsumer
{
    private readonly Lazy<EventingBasicConsumer> _consumer;

    private EventingBasicConsumer Consumer => _consumer.Value;

    protected MessageBrokerConsumerBase(
        ILogger<MessageBrokerBase> logger,
        MessageBrokerConnection connection)
        : base(logger, connection)
    {
        _consumer = new Lazy<EventingBasicConsumer>(new EventingBasicConsumer(Channel));
    }

    public void AddListener(EventHandler<BasicDeliverEventArgs> handler)
    {
        Consumer.Received += handler;
        Channel.BasicConsume(QueueKey, true, Guid.NewGuid().ToString(), false, false, null, Consumer);
    }

    public void RemoveListener(EventHandler<BasicDeliverEventArgs> handler)
    {
        Consumer.Received -= handler;
    }
}