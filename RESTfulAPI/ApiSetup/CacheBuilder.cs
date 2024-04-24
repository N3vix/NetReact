using RESTfulAPI.Caching;

namespace RESTfulAPI.ApiSetup;

internal static class CacheBuilder
{
    public static void SetupCache(this IServiceCollection services, IConfigurationManager config)
    {
        services.AddSingleton<CacheConnection>();
        services.AddScoped<ICacheService, CacheServiceStub>();
    }

    private class CacheServiceStub : ICacheService
    {
        public async Task<T> GetData<T>(string key)
        {
            return default;
        }

        public async Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            return false;
        }

        public async Task<object> RemoveData(string key)
        {
            throw new NotImplementedException();
        }
    }
}
