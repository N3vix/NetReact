using RESTfulAPI.Caching;

namespace RESTfulAPI.ApiSetup;

internal static class CacheBuilder
{
    public static void SetupCache(this IServiceCollection services, IConfigurationManager config)
    {
        services.AddSingleton<CacheConnection>();
        services.AddScoped<ICacheService, CacheService>();
    }
}
