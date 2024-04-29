using NetReactMonolith.Repositories;

namespace NetReactMonolith.ApiSetup;

internal static class ApplicationContextSetup
{
    public static void SetupApplicationContext(this IServiceCollection services, IConfigurationManager config)
    {
        services.AddSingleton<IConnectionsRepository, ConnectionsRepository>();
    }
}