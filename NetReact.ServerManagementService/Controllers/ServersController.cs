using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using NetReact.ServerManagementService.Gateways;

namespace NetReact.ServerManagementService.Controllers;

[ApiController]
[Authorize]
[Route("servers")]
public class ServersController : ControllerBase
{
    private ILogger<ServersController> Logger { get; }
    private IServersGateway ServersGateway { get; }

    public ServersController(ILogger<ServersController> logger, IServersGateway serversGateway)
    {
        Logger = logger;
        ServersGateway = serversGateway;
    }

    [HttpPost("")]
    public async Task<string> CreateServer([FromBody] ServerAddRequest request)
    {
        return await ServersGateway.CreateServer(request.Name);
    }

    [HttpGet("")]
    public async Task<IEnumerable<ServerDetails>> GetAllServers()
    {
        return await ServersGateway.GetAllServers();
    }

    [HttpGet("user")]
    public async Task<IEnumerable<ServerDetails>> GetFollowingServers()
    {
        return await ServersGateway.GetFollowedServers(User.GetUserId());
    }

    [HttpGet("{id}")]
    public async Task<ServerDetails> GetServer(string id)
    {
        return await ServersGateway.GetServer(id);
    }

    [HttpGet("{serverId}/user")]
    public async Task<bool> GetIsFollowing(string serverId)
    {
        return await ServersGateway.GetIsFollowing(User.GetUserId(), serverId);
    }
}