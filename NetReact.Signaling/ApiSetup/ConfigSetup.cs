using NetReact.MessageBroker.SharedModels;

namespace NetReact.Signaling.ApiSetup;

internal static class ConfigSetup
{
    public static void SetupConfigs(this IServiceCollection serviceCollection, IConfigurationManager config)
    {
        serviceCollection.Configure<MessageBrokerConnectionConfig>(config.GetSection("MessageBrokerConnection"));
        serviceCollection.Configure<MessageBrokerChannelConnectionConfig>(
            "MessageCreatedCommand",
            config.GetSection("MessageBrokerChannelConnections:MessageCreatedCommand"));
        serviceCollection.Configure<MessageBrokerChannelConnectionConfig>(
            "MessageEditedCommand",
            config.GetSection("MessageBrokerChannelConnections:MessageEditedCommand"));
        serviceCollection.Configure<MessageBrokerChannelConnectionConfig>(
            "MessageDeletedCommand",
            config.GetSection("MessageBrokerChannelConnections:MessageDeletedCommand"));
    }
}