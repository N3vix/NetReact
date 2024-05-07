using NetReact.ServerManagementService.Services;

namespace NetReact.ServerManagementService.ApiSetup;

internal static class DataGatewaysSetup
{
    public static void SetupGateways(this IServiceCollection services)
    {
        services.AddScoped<IServersService, ServersService>();
    }
}