using Microsoft.AspNetCore.Mvc;
using Models;
using RESTfulAPI.Repositories;

namespace RESTfulAPI.Gateways;

internal class ServersGateway : IServersGateway
{
    private IServersRepository ServersRepository { get; }
    private ICacheService CacheService { get; }

    public ServersGateway(IServersRepository serversRepository, ICacheService cacheService)
    {
        ServersRepository = serversRepository;
        CacheService = cacheService;
    }

    public async Task<IEnumerable<ServerDetails>> GetAllServers()
    {
        var cacheData = await CacheService.GetData<IEnumerable<ServerDetails>>(nameof(GetAllServers));
        if (cacheData != null)
            return cacheData;

        var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);

        cacheData = await ServersRepository.Get();
        CacheService.SetData(nameof(GetAllServers), cacheData, expirationTime);

        return cacheData;
    }

    public async Task<IEnumerable<ServerDetails>> GetFollowedServers(string userId)
    {
        var cacheData = await CacheService.GetData<IEnumerable<ServerDetails>>(nameof(GetFollowedServers));
        if (cacheData != null)
            return cacheData;

        var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);

        cacheData = await ServersRepository.GetByUserId(userId);
        CacheService.SetData(nameof(GetFollowedServers), cacheData, expirationTime);

        return cacheData;
    }

    public async Task<ServerDetails> GetServer([FromQuery] string id)
    {
        return await ServersRepository.GetById(id);
    }
}
