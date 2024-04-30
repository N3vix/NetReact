using Microsoft.EntityFrameworkCore;
using NetReact.AuthService.DB;

namespace NetReact.AuthService.ApiSetup;

internal static class ApplicationContextBuilder
{
    public static void SetupApplicationContext(this IServiceCollection services, IConfigurationManager config)
    {
        var connections = new Connections();
        config.GetSection(nameof(Connections)).Bind(connections);
        services.AddDbContext<ApplicationContext>(options => ConfigureApplicationContextOptions(options, connections));
    }

    private static void ConfigureApplicationContextOptions(
        DbContextOptionsBuilder options,
        Connections connections)
        => options.UseMySQL(connections.Database);
}