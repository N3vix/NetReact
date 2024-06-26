﻿using StackExchange.Redis;

namespace NetReact.ServerManagementService.Caching;

public class CacheConnection
{
    public IDatabase Db { get; }

    public CacheConnection(IConfiguration configuration)
    {
        var redisUrl = configuration.GetValue<string>("RedisURL");
        Db = ConnectionMultiplexer.Connect(redisUrl).GetDatabase();
    }
}
