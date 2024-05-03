using NetReact.Signaling.Repositories;

namespace NetReact.Signaling.ApiSetup;

internal static class ApplicationContextSetup
{
    public static void SetupApplicationContext(this IServiceCollection services, IConfigurationManager config)
    {
        services.AddSingleton<IConnectionsRepository, ConnectionsRepository>();
    }
}