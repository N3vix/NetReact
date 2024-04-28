using NetReactMonolith.DB;
using NetReactMonolith.Repositories;
using NetReactMonolith.Repositories.MongoDB;

namespace NetReactMonolith.ApiSetup;

internal static class ApplicationContextSetup
{
    public static void SetupApplicationContext(this IServiceCollection services, IConfigurationManager config)
    {
        services.AddSingleton<IMongoDbContext, MongoDbContext>(_ =>
            new MongoDbContext(config.GetConnectionString("MongoDB")));

        services.AddSingleton<IConnectionsRepository, ConnectionsRepository>();
        services.AddScoped<IServersRepository, ServersRepositoryMongoDB>();
        services.AddScoped<IServerFollowersRepository, ServerFollowersRepositoryMongoDB>();
        services.AddScoped<IChannelsRepository, ChannelsRepositoryMongoDB>();
        services.AddScoped<IChannelMessagesRepository, ChannelMessagesRepositoryMongoDb>();
    }
}