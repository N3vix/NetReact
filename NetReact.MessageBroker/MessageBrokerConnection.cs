using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace NetReact.MessageBroker;

public class MessageBrokerConnection : IDisposable
{
    private readonly ILogger<MessageBrokerConnection> _logger;
    private readonly MessageBrokerConnectionConfig _connectionConfig;

    public IConnection Connection { get; }

    public MessageBrokerConnection(
        ILogger<MessageBrokerConnection> logger,
        IOptions<MessageBrokerConnectionConfig> connectionConfig)
    {
        _logger = logger;
        _connectionConfig = connectionConfig.Value;

        var factory = GetConnectionFactory();
        try
        {
            Connection = factory.CreateConnection();
            Connection.ConnectionShutdown += Connection_ConnectionShutdown;
            logger.LogInformation("Connection has been created");
        }
        catch (Exception e)
        {
            _logger.LogCritical($"Could not connect to the rabbitmq: {e.Message}");
        }
    }

    private ConnectionFactory GetConnectionFactory()
    {
        return new ConnectionFactory
        {
            HostName = _connectionConfig.HostName,
            Port = _connectionConfig.Port,
            UserName = _connectionConfig.UserName,
            Password = _connectionConfig.Password,
            VirtualHost = "/"
        };
    }

    private void Connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        _logger.LogInformation("Connection has been shutdown");
    }

    public void Dispose()
    {
        Connection.ConnectionShutdown -= Connection_ConnectionShutdown;
        Connection.Dispose();
    }
}