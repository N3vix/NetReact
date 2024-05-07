using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace NetReact.ServiceSetup;

public static class AuthenticationServiceExtensions
{
    public static void SetupAuthentication(this IServiceCollection services, IConfigurationManager config)
    {
        services.Configure<JwtConfig>(config.GetSection("Authentication:Schemes:Bearer"));
        
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
        options.Events = GetJwtBearerEvents();
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
    
    private static JwtBearerEvents GetJwtBearerEvents() => new() { OnMessageReceived = PopulateAccessToken };

    private static Task PopulateAccessToken(MessageReceivedContext messageContext)
    {
        var accessToken = messageContext.Request.Query["access_token"];
        var path = messageContext.HttpContext.Request.Path;
        if (!string.IsNullOrEmpty(accessToken)
            && path.StartsWithSegments("/chat"))
        {
            messageContext.Token = accessToken;
        }
        return Task.CompletedTask;
    }
}