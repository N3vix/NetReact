using RESTfulAPI.Gateways;

namespace RESTfulAPI.ApiSetup;

internal static class DataGatewaysBuilder
{
    public static void SetupGateways(this IServiceCollection services)
    {
        services.AddScoped<IChannelMessagesGateway, ChannelMessagesGateway>();
        services.AddScoped<IMessageMediaGetaway, MessageMediaGetaway>();
    }
}
