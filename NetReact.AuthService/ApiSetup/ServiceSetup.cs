using NetReact.AuthService.Services;

namespace NetReact.AuthService.ApiSetup;

internal static class ServicesSetup
{
    public static void SetupServices(this IServiceCollection services)
    {
        services.AddScoped<IdentityService>();
    }
}