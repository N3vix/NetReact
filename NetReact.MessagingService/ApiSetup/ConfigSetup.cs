using NetReact.MessageBroker.SharedModels;

namespace NetReact.MessagingService.ApiSetup;

internal static class ConfigSetup
{
    public static void SetupConfigs(this IServiceCollection serviceCollection, IConfigurationManager config)
    {
        serviceCollection.Configure<Connections>(config.GetSection("Connections"));
        serviceCollection.Configure<MessageBrokerConnectionConfig>(config.GetSection("MessageBrokerConnection"));
        serviceCollection.Configure<MessageBrokerChannelConnectionConfig>(
            "MessageCreateCommand",
            config.GetSection("MessageBrokerChannelConnections:MessageCreateCommand"));
        serviceCollection.Configure<MessageBrokerChannelConnectionConfig>(
            "MessageEditCommand",
            config.GetSection("MessageBrokerChannelConnections:MessageEditCommand"));
        serviceCollection.Configure<MessageBrokerChannelConnectionConfig>(
            "MessageDeleteCommand",
            config.GetSection("MessageBrokerChannelConnections:MessageDeleteCommand"));
    }
}