using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetReact.MessageBroker.SharedModels;
using OpenTelemetry.Trace;

namespace NetReact.MessageBroker;

public class MessageBrokerConsumerFactory : IMessageBrokerConsumerFactory
{
    private readonly ILogger<IMessageBrokerConsumer> _logger;
    private readonly Tracer _tracer;
    private readonly MessageBrokerConnection _connection;

    public MessageBrokerConsumerFactory(
        ILogger<IMessageBrokerConsumer> logger,
        Tracer tracer,
        MessageBrokerConnection connection)
    {
        ArgumentNullException.ThrowIfNull(connection);

        _logger = logger;
        _tracer = tracer;
        _connection = connection;
    }

    public IMessageBrokerConsumer Build(IMessageConsumerHandler consumerHandler)
    {
        return new MessageBrokerConsumer(_tracer, _logger, _connection, consumerHandler);
    }
}