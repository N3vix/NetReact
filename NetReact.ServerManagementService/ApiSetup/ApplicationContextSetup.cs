using NetReact.ServerManagementService.DB;
using NetReact.ServerManagementService.Repositories;

namespace NetReact.ServerManagementService.ApiSetup;

internal static class ApplicationContextSetup
{
    public static void SetupApplicationContext(this IServiceCollection services, IConfigurationManager config)
    {
        var connections = new Connections();
        config.GetSection(nameof(Connections)).Bind(connections);
        
        services.AddSingleton<IMongoDbContext, MongoDbContext>(_ => new MongoDbContext(connections.Database));
        
        services.AddScoped<IServersRepository, ServersRepository>();
        services.AddScoped<IServerFollowersRepository, ServerFollowersRepository>();
    }
}