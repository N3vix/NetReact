using Microsoft.EntityFrameworkCore;
using NetReact.AuthService.DB;

namespace NetReact.AuthService.ApiSetup;

internal static class ApplicationContextBuilder
{
    public static void SetupApplicationContext(this IServiceCollection services, IConfigurationManager config)
    {
        services.AddDbContext<ApplicationContext>(options => ConfigureApplicationContextOptions(options, config));
    }

    private static void ConfigureApplicationContextOptions(DbContextOptionsBuilder options,
        IConfigurationManager config)
        => options.UseSqlite(config.GetConnectionString("Database"));
}