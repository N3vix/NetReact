using Microsoft.EntityFrameworkCore;
using NetReact.ChannelManagementService.DB;
using NetReact.ChannelManagementService.Repositories;

namespace NetReact.ChannelManagementService.ApiSetup;

internal static class ApplicationContextSetup
{
    public static void SetupApplicationContext(this IServiceCollection services, IConfigurationManager config)
    {
        var connections = new Connections();
        config.GetSection(nameof(Connections)).Bind(connections);

        services.AddDbContext<ApplicationContext>(options => ConfigureApplicationContextOptions(options, connections));

        services.AddScoped<IChannelsRepository, ChannelsRepository>();
    }

    private static void ConfigureApplicationContextOptions(
        DbContextOptionsBuilder options,
        Connections connections)
        => options.UseSqlServer(connections.Database);
}