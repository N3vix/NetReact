using Microsoft.Extensions.Logging;
using NetReact.MessageBroker.SharedModels;

namespace NetReact.MessageBroker;

public class MessageBrokerProducerFactory : IMessageBrokerProducerFactory
{
    private readonly ILogger _logger;
    private readonly MessageBrokerConnection _connection;

    public MessageBrokerProducerFactory(ILogger<IMessageBrokerProducer> logger, MessageBrokerConnection connection)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(connection);
        
        _logger = logger;
        _connection = connection;
    }

    public IMessageBrokerProducer Build(MessageBrokerChannelConnectionConfig channelConnectionConfig)
    {
        return new MessageBrokerProducer(_logger, _connection, channelConnectionConfig);
    }
}