using NetReact.ChannelManagementService.DB;
using NetReact.ChannelManagementService.Repositories;

namespace NetReact.ChannelManagementService.ApiSetup;

internal static class ApplicationContextSetup
{
    public static void SetupApplicationContext(this IServiceCollection services, IConfigurationManager config)
    {
        services.AddSingleton<IMongoDbContext, MongoDbContext>(_ =>
            new MongoDbContext(config.GetConnectionString("MongoDB")));

        services.AddScoped<IChannelsRepository, ChannelsRepository>();
    }
}