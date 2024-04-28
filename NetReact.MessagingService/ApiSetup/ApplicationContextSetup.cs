using NetReact.MessagingService.DB;
using NetReact.MessagingService.Repositories;

namespace NetReact.MessagingService.ApiSetup;

internal static class ApplicationContextSetup
{
    public static void SetupApplicationContext(this IServiceCollection services, IConfigurationManager config)
    {
        services.AddSingleton<IMongoDbContext, MongoDbContext>(_ =>
            new MongoDbContext(config.GetConnectionString("MongoDB")));

        services.AddScoped<IMessagesRepository, MessagesRepository>();
    }
}