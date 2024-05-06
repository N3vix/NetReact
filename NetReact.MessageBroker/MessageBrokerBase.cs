using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace NetReact.MessageBroker;

public abstract class MessageBrokerBase
{
    private readonly ILogger<MessageBrokerBase> _logger;
    private readonly MessageBrokerConnection _connection;

    protected abstract string ExchangeKey { get; }
    protected abstract string QueueKey { get; }
    protected abstract string RoutingKey { get; }

    private readonly Lazy<IModel> _channel;

    protected IModel Channel => _channel.Value;

    protected MessageBrokerBase(
        ILogger<MessageBrokerBase> logger,
        MessageBrokerConnection connection)
    {
        _logger = logger;
        _connection = connection;

        _channel = new Lazy<IModel>(InitChannel);
    }

    private IModel InitChannel()
    {
        try
        {
            var channel = _connection.Connection.CreateModel();
            channel.ExchangeDeclare(ExchangeKey, ExchangeType.Direct);
            channel.QueueDeclare(QueueKey, true, false);
            channel.QueueBind(QueueKey, ExchangeKey, RoutingKey);
            _logger.LogInformation("Connection has been created");
            return channel;
        }
        catch (Exception e)
        {
            _logger.LogCritical($"Could not connect to the channel: {e.Message}");
            return null;
        }
    }
}