using NetReact.ChannelManagementService.Gateways;

namespace NetReact.ChannelManagementService.ApiSetup;

internal static class DataGatewaysSetup
{
    public static void SetupGateways(this IServiceCollection services)
    {
        services.AddScoped<IChannelsGateway, ChannelsGateway>();
    }
}