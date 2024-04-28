using System.Text.Json;

namespace NetReactMonolith.Caching;

internal class CacheService : ICacheService
{
    private CacheConnection CacheConnection { get; }

    public CacheService(CacheConnection configuration)
    {
        CacheConnection = configuration;
    }

    public async Task<T> GetData<T>(string key)
    {
        var value = await CacheConnection.Db.StringGetAsync(key);
        return string.IsNullOrEmpty(value) 
            ? default 
            : JsonSerializer.Deserialize<T>(value);
    }

    public async Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
        var isSet = await CacheConnection.Db.StringSetAsync(key, JsonSerializer.Serialize(value), expiryTime);
        return isSet;
    }

    public async Task<object> RemoveData(string key)
    {
        var _isKeyExist = CacheConnection.Db.KeyExists(key);
        if (_isKeyExist)
        {
            return await CacheConnection.Db.KeyDeleteAsync(key);
        }
        return false;
    }
}
