using Microsoft.AspNetCore.Identity;
using NetReact.AuthService.DB;

namespace NetReact.AuthService.ApiSetup;

internal static class AuthorisationServiceBuilder
{
    public static void SetupAuthorisation(this IServiceCollection services)
    {
        services
            .AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<ApplicationContext>();
    }
}