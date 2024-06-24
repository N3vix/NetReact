using Microsoft.EntityFrameworkCore;
using NetReact.MessagingWorker.DB;
using NetReact.MessagingWorker.Repositories;

namespace NetReact.MessagingWorker.ApiSetup;

internal static class ApplicationContextSetup
{
    public static void SetupApplicationContext(this IServiceCollection services, IConfigurationManager config)
    {
        var connections = new Connections();
        config.GetSection(nameof(Connections)).Bind(connections);

        services.AddDbContext<ApplicationContext>(options => ConfigureApplicationContextOptions(options, connections));
        
        services.AddScoped<IMessagesRepository, MessagesRepository>();
    }    
    
    private static void ConfigureApplicationContextOptions(
        DbContextOptionsBuilder options,
        Connections connections)
        => options.UseSqlServer(connections.Database);
}