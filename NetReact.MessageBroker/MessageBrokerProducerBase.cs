using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace NetReact.MessageBroker;

public abstract class MessageBrokerProducerBase : MessageBrokerBase, IMessageProducer
{
    protected MessageBrokerProducerBase(
        ILogger<MessageBrokerBase> logger, 
        MessageBrokerConnection connection) 
        : base(logger, connection)
    { }

    public void SendMessage<T>(T message)
    {
        var body = SerializeMessage(message);

        Channel.BasicPublish(ExchangeKey, RoutingKey, body: body);
    }

    private byte[] SerializeMessage<T>(T message)
    {
        var jsonMessage = JsonSerializer.Serialize(message);
        return Encoding.UTF8.GetBytes(jsonMessage);
    }
}