using Microsoft.Extensions.DependencyInjection;

namespace NetReact.ServiceSetup;

public static class CorsExtensions
{
    public static void SetupCors(this IServiceCollection services)
    {
        services.AddCors(opt =>
        {
            opt.AddPolicy("reactApp", builder =>
            {
                builder
                .WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });
        });
    }
}
