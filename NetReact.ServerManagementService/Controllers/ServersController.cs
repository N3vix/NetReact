using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using NetReact.ServerManagementService.Gateways;

namespace NetReact.ServerManagementService.Controllers;

[ApiController]
[Route("[controller]")]
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

    [HttpGet("[action]")]
    public async Task<string> CreateServer([FromQuery] string name)
    {
        return await ServersGateway.CreateServer(name);
    }

    [HttpGet("[action]")]
    public async Task<IEnumerable<ServerDetails>> GetAllServers()
    {
        return await ServersGateway.GetAllServers();
    }

    [HttpGet("[action]")]
    public async Task<IEnumerable<ServerDetails>> GetAddedServers()
    {
        return await ServersGateway.GetFollowedServers(User.GetUserId());
    }

    [HttpGet("[action]")]
    public async Task<ServerDetails> GetServer([FromQuery] string id)
    {
        return await ServersGateway.GetServer(id);
    }

    [HttpGet("[action]")]
    public async Task<bool> GetIsFollowing([FromQuery] string userId, [FromQuery] string serverId)
    {
        return await ServersGateway.GetIsFollowing(userId, serverId);
    }
}