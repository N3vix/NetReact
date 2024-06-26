using Microsoft.Extensions.Logging;
using NetReact.MessageBroker.SharedModels;
using OpenTelemetry.Trace;

namespace NetReact.MessageBroker;

public class MessageBrokerConsumerFactory : IMessageBrokerConsumerFactory
{
    private readonly Tracer _tracer;
    private readonly ILogger _logger;
    private readonly MessageBrokerConnection _connection;

    public MessageBrokerConsumerFactory(
        Tracer tracer,
        ILogger<IMessageBrokerConsumer> logger,
        MessageBrokerConnection connection)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(connection);

        _tracer = tracer;
        _logger = logger;
        _connection = connection;
    }

    public IMessageBrokerConsumer Build(MessageBrokerChannelConnectionConfig channelConnectionConfig)
    {
        return new MessageBrokerConsumer(_tracer, _logger, _connection, channelConnectionConfig);
    }
}