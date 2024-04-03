using RESTfulAPI.Gateways;

namespace RESTfulAPI.ApiSetup;

internal static class DataGatewaysBuilder
{
    public static void SetupGateways(this IServiceCollection services)
    {
        services.AddScoped<IServersGateway, ServersGateway>();
        services.AddScoped<IChannelMessagesGateway, ChannelMessagesGateway>();
        services.AddScoped<IMessageMediaGetaway, MessageMediaGetaway>();
    }
}