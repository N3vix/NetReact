using Microsoft.Extensions.Logging;
using NetReact.MessageBroker.SharedModels;
using OpenTelemetry.Trace;

namespace NetReact.MessageBroker;

public class MessageBrokerProducerFactory : IMessageBrokerProducerFactory
{
    private readonly Tracer _tracer;
    private readonly ILogger _logger;
    private readonly MessageBrokerConnection _connection;

    public MessageBrokerProducerFactory(
        Tracer tracer, 
        ILogger<IMessageBrokerProducer> logger, 
        MessageBrokerConnection connection)
    {
        ArgumentNullException.ThrowIfNull(tracer);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(connection);

        _tracer = tracer;
        _logger = logger;
        _connection = connection;
    }

    public IMessageBrokerProducer Build(MessageBrokerChannelConnectionConfig channelConnectionConfig)
    {
        return new MessageBrokerProducer(_tracer, _logger, _connection, channelConnectionConfig);
    }
}