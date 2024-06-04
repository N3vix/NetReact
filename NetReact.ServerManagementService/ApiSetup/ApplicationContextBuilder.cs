using Microsoft.EntityFrameworkCore;
using NetReact.ServerManagementService.DB;
using NetReact.ServerManagementService.Repositories;

namespace NetReact.ServerManagementService.ApiSetup;

internal static class ApplicationContextBuilder
{
    public static void SetupApplicationContext(this IServiceCollection services, IConfigurationManager config)
    {
        var connections = new Connections();
        config.GetSection(nameof(Connections)).Bind(connections);

        services.AddDbContext<ApplicationContext>(options => ConfigureApplicationContextOptions(options, connections));

        services.AddScoped<IServersRepository, ServersRepository>();
    }

    private static void ConfigureApplicationContextOptions(
        DbContextOptionsBuilder options,
        Connections connections)
        => options.UseSqlServer(connections.Database);
}