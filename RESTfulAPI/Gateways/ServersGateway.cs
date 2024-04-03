using Microsoft.AspNetCore.Mvc;
using Models;
using RESTfulAPI.Repositories;

namespace RESTfulAPI.Gateways;

internal class ServersGateway : IServersGateway
{
    private IServersRepository ServersRepository { get; }

    public ServersGateway(IServersRepository serversRepository)
    {
        ServersRepository = serversRepository;
    }

    public async Task<IEnumerable<ServerDetails>> GetAllServers()
    {
        return await ServersRepository.Get();
    }

    public async Task<IEnumerable<ServerDetails>> GetFollowedServers(string userId)
    {
        return await ServersRepository.GetByUserId(userId);
    }

    public async Task<ServerDetails> GetServer([FromQuery] string id)
    {
        return await ServersRepository.GetById(id);
    }
}
