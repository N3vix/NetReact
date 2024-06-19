using Microsoft.AspNetCore.Mvc;
using Models;
using NetReact.ServerManagementService.Caching;
using NetReact.ServerManagementService.Repositories;
using OpenTelemetry.Trace;

namespace NetReact.ServerManagementService.Services;

internal class ServersService : IServersService
{
    private readonly Tracer _tracer;
    private readonly IServersRepository _serversRepository;
    private readonly ICacheService _cacheService;

    public ServersService(
        Tracer tracer,
        IServersRepository serversRepository,
        ICacheService cacheService)
    {
        ArgumentNullException.ThrowIfNull(tracer);
        ArgumentNullException.ThrowIfNull(serversRepository);
        ArgumentNullException.ThrowIfNull(cacheService);

        _tracer = tracer;
        _serversRepository = serversRepository;
        _cacheService = cacheService;
    }

    public async Task<string> CreateServer(string name)
    {
        using var _ = _tracer.StartSpan(nameof(CreateServer));
        var id = await _serversRepository.Add(new ServerDetails { Name = name });
        await _serversRepository.Save();
        return id;
    }

    public async Task<IEnumerable<ServerDetails>> GetAllServers()
    {
        using var _ = _tracer.StartSpan(nameof(GetAllServers));
        var cacheData = await _cacheService.GetData<IEnumerable<ServerDetails>>(nameof(GetAllServers));
        if (cacheData != null)
            return cacheData;

        var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);

        cacheData = await _serversRepository.Get();
        await _cacheService.SetData(nameof(GetAllServers), cacheData, expirationTime);

        return cacheData;
    }

    public async Task<IEnumerable<ServerDetails>> GetFollowedServers(string userId)
    {
        using var _ = _tracer.StartSpan(nameof(GetFollowedServers));
        var cacheData =
            await _cacheService.GetData<IEnumerable<ServerDetails>>($"{nameof(GetFollowedServers)}{userId}");
        if (cacheData != null)
            return cacheData;

        var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);

        cacheData = await _serversRepository.GetByUserId(userId);
        await _cacheService.SetData(nameof(GetFollowedServers), cacheData, expirationTime);

        return cacheData;
    }

    public async Task<ServerDetails> GetServer([FromQuery] string id)
    {
        using var _ = _tracer.StartSpan(nameof(GetServer));
        return await _serversRepository.GetById(id);
    }

    public async Task<bool> GetIsFollowing(string userId, string serverId)
    {
        using var _ = _tracer.StartSpan(nameof(GetIsFollowing));
        return await _serversRepository.GetIsUserFollowingServer(userId, serverId);
    }
}