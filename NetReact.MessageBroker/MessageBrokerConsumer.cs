using Microsoft.Extensions.Logging;
using NetReact.MessageBroker.SharedModels;
using OpenTelemetry.Trace;
using RabbitMQ.Client.Events;

namespace NetReact.MessageBroker;

internal class MessageBrokerConsumer : MessageBroker, IMessageBrokerConsumer
{
    private readonly Tracer _tracer;
    private readonly EventingBasicConsumer _consumer;

    public MessageBrokerConsumer(
        Tracer tracer,
        ILogger logger,
        MessageBrokerConnection connection,
        MessageBrokerChannelConnectionConfig channelConnectionConfig)
        : base(logger, connection, channelConnectionConfig)
    {
        _tracer = tracer;
        _consumer = new EventingBasicConsumer(Channel);
    }

    public void AddListener(EventHandler<BasicDeliverEventArgs> handler)
    {
        _consumer.Received += handler;
        Channel.BasicConsume(
            ChannelConnectionConfig.Queue,
            true, Guid.NewGuid().ToString(), false, false, null,
            _consumer);
    }

    public void RemoveListener(EventHandler<BasicDeliverEventArgs> handler)
    {
        _consumer.Received -= handler;
    }
}