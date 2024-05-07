using Microsoft.Extensions.Logging;
using NetReact.MessageBroker.SharedModels;

namespace NetReact.MessageBroker;

public class MessageBrokerConsumerFactory : IMessageBrokerConsumerFactory
{
    private readonly ILogger _logger;
    private readonly MessageBrokerConnection _connection;

    public MessageBrokerConsumerFactory(ILogger<IMessageBrokerConsumer> logger, MessageBrokerConnection connection)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(connection);
        
        _logger = logger;
        _connection = connection;
    }

    public IMessageBrokerConsumer Build(MessageBrokerChannelConnectionConfig channelConnectionConfig)
    {
        return new MessageBrokerConsumer(_logger, _connection, channelConnectionConfig);
    }
}