using NetReactMonolith.Gateways;

namespace NetReactMonolith.ApiSetup;

internal static class DataGatewaysSetup
{
    public static void SetupGateways(this IServiceCollection services)
    {
        services.AddScoped<IServersGateway, ServersGateway>();
        services.AddScoped<IChannelMessagesGateway, ChannelMessagesGateway>();
        services.AddScoped<IMessageMediaGetaway, MessageMediaGetaway>();
    }
}