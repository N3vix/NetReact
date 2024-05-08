using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NetReact.MessageBroker.SharedModels;
using RabbitMQ.Client;

namespace NetReact.MessageBroker;

internal class MessageBrokerProducer : MessageBroker, IMessageBrokerProducer
{
    public MessageBrokerProducer(
        ILogger logger,
        MessageBrokerConnection connection,
        MessageBrokerChannelConnectionConfig channelConnectionConfig)
        : base(logger, connection, channelConnectionConfig)
    { }

    public void SendMessage<T>(T message)
    {
        var body = SerializeMessage(message);

        Channel.BasicPublish(ChannelConnectionConfig.Exchange, ChannelConnectionConfig.Routing, body: body);
    }

    private byte[] SerializeMessage<T>(T message)
    {
        var jsonMessage = JsonSerializer.Serialize(message);
        return Encoding.UTF8.GetBytes(jsonMessage);
    }
}