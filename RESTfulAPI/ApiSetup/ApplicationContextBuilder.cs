using RESTfulAPI.DB;
using RESTfulAPI.Repositories.MongoDB;
using RESTfulAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace RESTfulAPI.ApiSetup;

internal static class ApplicationContextBuilder
{
    public static void SetupApplicationContext(this IServiceCollection services, IConfigurationManager config)
    {
        services.AddSingleton<IMongoDbContext, MongoDbContext>(_ =>
            new MongoDbContext(config.GetConnectionString("MongoDB")));

        services.AddDbContext<ApplicationContext>(options => ConfigureApplicationContextOptions(options, config));

        services.AddSingleton<IMessagesRepository, MessagesRepository>();
        services.AddScoped<IServersRepository, ServersRepositoryMongoDB>();
        services.AddScoped<IServerFollowersRepository, ServerFollowersRepositoryMongoDB>();
        services.AddScoped<IChannelsRepository, ChannelsRepositoryMongoDB>();
        services.AddScoped<IChannelMessagesRepository, ChannelMessagesRepositoryMongoDb>();
    }

    private static void ConfigureApplicationContextOptions(DbContextOptionsBuilder options,
        IConfigurationManager config)
        => options.UseSqlite(config.GetConnectionString("Database"));
}