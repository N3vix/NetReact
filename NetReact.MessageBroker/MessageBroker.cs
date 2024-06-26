using Microsoft.Extensions.Logging;
using NetReact.MessageBroker.SharedModels;
using RabbitMQ.Client;

namespace NetReact.MessageBroker;

internal abstract class MessageBroker : IDisposable
{
    protected readonly ILogger _logger;
    private readonly MessageBrokerConnection _connection;
    protected readonly MessageBrokerChannelConnectionConfig ChannelConnectionConfig;

    protected IModel Channel { get; }

    protected MessageBroker(
        ILogger logger,
        MessageBrokerConnection connection,
        MessageBrokerChannelConnectionConfig channelConnectionConfig)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(channelConnectionConfig);
        
        _logger = logger;
        _connection = connection;
        ChannelConnectionConfig = channelConnectionConfig;

        Channel = InitChannel();
    }

    private IModel InitChannel()
    {
        try
        {
            var channel = _connection.Connection.CreateModel();
            channel.ExchangeDeclare(ChannelConnectionConfig.Exchange, ExchangeType.Direct);
            channel.QueueDeclare(ChannelConnectionConfig.Queue, true, false);
            channel.QueueBind(
                ChannelConnectionConfig.Queue,
                ChannelConnectionConfig.Exchange,
                ChannelConnectionConfig.Routing);
            _logger.LogInformation("Connection has been created");
            return channel;
        }
        catch (Exception e)
        {
            _logger.LogCritical($"Could not connect to the channel: {e.Message}");
            return null;
        }
    }

    public void Dispose()
    {
        Channel.Dispose();
    }
}