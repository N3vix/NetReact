using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NetReact.AuthService.Configurations;
using NetReact.AuthService.DB;

namespace NetReact.AuthService.ApiSetup;

internal static class AuthorisationServiceBuilder
{
    public static void SetupAuthorisation(this IServiceCollection services, IConfigurationManager config)
    {
        services.Configure<JwtConfig>(config.GetSection("Authentication:Schemes:Bearer"));

        services
            .AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<ApplicationContext>();
        services
            .AddAuthentication(ConfigureAuthentication)
            .AddJwtBearer(options => ConfigureJwtBearer(options, config));
        services.AddAuthorization();
    }

    private static void ConfigureAuthentication(Microsoft.AspNetCore.Authentication.AuthenticationOptions options)
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }

    private static void ConfigureJwtBearer(JwtBearerOptions options, IConfigurationManager config)
    {
        options.SaveToken = true;
        options.TokenValidationParameters = GetTokenValidationParameters(config);
    }

    private static TokenValidationParameters GetTokenValidationParameters(IConfigurationManager config)
    {
        return new TokenValidationParameters
        {
            ValidIssuer = config["Authentication:Schemes:Bearer:ValidIssuer"],
            ValidAudiences = config.GetSection("Authentication:Schemes:Bearer:ValidAudiences").Get<string[]>(),
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["Authentication:Schemes:Bearer:ValidKey"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
        };
    }
}