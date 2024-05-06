using NetReact.MessagingWorker.DB;
using NetReact.MessagingWorker.Repositories;

namespace NetReact.MessagingWorker.ApiSetup;

internal static class ApplicationContextSetup
{
    public static void SetupApplicationContext(this IServiceCollection services, IConfigurationManager config)
    {
        var connections = new Connections();
        config.GetSection(nameof(Connections)).Bind(connections);
        
        services.AddSingleton<IMongoDbContext, MongoDbContext>(_ => new MongoDbContext(connections.Database));
        
        services.AddScoped<IMessagesRepository, MessagesRepository>();
    }
}