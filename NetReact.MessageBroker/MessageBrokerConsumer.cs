using Microsoft.Extensions.Logging;
using NetReact.MessageBroker.SharedModels;
using RabbitMQ.Client.Events;

namespace NetReact.MessageBroker;

internal class MessageBrokerConsumer : MessageBroker, IMessageBrokerConsumer
{
    private readonly EventingBasicConsumer _consumer;

    public MessageBrokerConsumer(
        ILogger logger,
        MessageBrokerConnection connection,
        MessageBrokerChannelConnectionConfig channelConnectionConfig)
        : base(logger, connection, channelConnectionConfig)
    {
        _consumer = new EventingBasicConsumer(Channel);
    }

    public void AddListener(EventHandler<BasicDeliverEventArgs> handler)
    {
        _consumer.Received += handler;
        Channel.BasicConsume(
            ChannelConnectionConfig.QueueKey,
            true, Guid.NewGuid().ToString(), false, false, null,
            _consumer);
    }

    public void RemoveListener(EventHandler<BasicDeliverEventArgs> handler)
    {
        _consumer.Received -= handler;
    }
}