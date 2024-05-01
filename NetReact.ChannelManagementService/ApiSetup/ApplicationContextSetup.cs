using NetReact.ChannelManagementService.DB;
using NetReact.ChannelManagementService.Repositories;

namespace NetReact.ChannelManagementService.ApiSetup;

internal static class ApplicationContextSetup
{
    public static void SetupApplicationContext(this IServiceCollection services, IConfigurationManager config)
    {
        var connections = new Connections();
        config.GetSection(nameof(Connections)).Bind(connections);

        services.AddSingleton<IMongoDbContext, MongoDbContext>(_ => new MongoDbContext(connections.Database));

        services.AddScoped<IChannelsRepository, ChannelsRepository>();
    }
}