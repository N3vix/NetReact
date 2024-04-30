using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using NetReact.ServerManagementService.Gateways;

namespace NetReact.ServerManagementService.Controllers;

[ApiController]
[Authorize]
public class ServersController : ControllerBase
{
    private ILogger<ServersController> Logger { get; }
    private IServersGateway ServersGateway { get; }

    public ServersController(ILogger<ServersController> logger, IServersGateway serversGateway)
    {
        Logger = logger;
        ServersGateway = serversGateway;
    }

    [HttpPost("servers")]
    public async Task<string> CreateServer([FromQuery] string name)
    {
        return await ServersGateway.CreateServer(name);
    }

    [HttpGet("servers")]
    public async Task<IEnumerable<ServerDetails>> GetAllServers()
    {
        return await ServersGateway.GetAllServers();
    }

    [HttpGet("servers/user")]
    public async Task<IEnumerable<ServerDetails>> GetFollowingServers()
    {
        return await ServersGateway.GetFollowedServers(User.GetUserId());
    }

    [HttpGet("servers/{id}")]
    public async Task<ServerDetails> GetServer(string id)
    {
        return await ServersGateway.GetServer(id);
    }

    [HttpGet("servers/{serverId}/user")]
    public async Task<bool> GetIsFollowing(string serverId)
    {
        return await ServersGateway.GetIsFollowing(User.GetUserId(), serverId);
    }
}