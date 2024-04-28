using NetReact.ServerManagementService.DB;
using NetReact.ServerManagementService.Repositories;

namespace NetReact.ServerManagementService.ApiSetup;

internal static class ApplicationContextSetup
{
    public static void SetupApplicationContext(this IServiceCollection services, IConfigurationManager config)
    {
        services.AddSingleton<IMongoDbContext, MongoDbContext>(_ =>
            new MongoDbContext(config.GetConnectionString("MongoDB")));

        services.AddScoped<IServersRepository, ServersRepository>();
        services.AddScoped<IServerFollowersRepository, ServerFollowersRepository>();
    }
}