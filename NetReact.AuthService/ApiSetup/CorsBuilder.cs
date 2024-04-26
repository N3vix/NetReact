namespace NetReact.AuthService.ApiSetup;

internal static class CorsBuilder
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
