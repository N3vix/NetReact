using NetReact.ServerManagementService.Gateways;

namespace NetReact.ServerManagementService.ApiSetup;

internal static class DataGatewaysSetup
{
    public static void SetupGateways(this IServiceCollection services)
    {
        services.AddScoped<IServersGateway, ServersGateway>();
    }
}