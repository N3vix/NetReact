using Newtonsoft.Json;
using StackExchange.Redis;

namespace RESTfulAPI;

public class CacheConnection
{
    public IDatabase Db { get; }

    public CacheConnection(IConfiguration configuration)
    {
        var redisUrl = configuration.GetValue<string>("RedisURL");
        Db = ConnectionMultiplexer.Connect(redisUrl).GetDatabase();
    }
}

public interface ICacheService
{
    Task<T> GetData<T>(string key);

    Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime);

    /// <returns></returns>
    Task<object> RemoveData(string key);
}

public class CacheService : ICacheService
{
    private CacheConnection CacheConnection { get; }

    public CacheService(CacheConnection configuration)
    {
        CacheConnection = configuration;
    }

    public async Task<T> GetData<T>(string key)
    {
        var value = await CacheConnection.Db.StringGetAsync(key);
        if (string.IsNullOrEmpty(value))
            return default;
        return JsonConvert.DeserializeObject<T>(value);
    }

    public async Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
        var isSet = await CacheConnection.Db.StringSetAsync(key, JsonConvert.SerializeObject(value), expiryTime);
        return isSet;
    }

    public async Task<object> RemoveData(string key)
    {
        bool _isKeyExist = CacheConnection.Db.KeyExists(key);
        if (_isKeyExist == true)
        {
            return await CacheConnection.Db.KeyDeleteAsync(key);
        }
        return false;
    }
}
