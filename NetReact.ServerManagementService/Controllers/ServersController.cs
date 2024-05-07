using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using NetReact.ServerManagementService.Services;

namespace NetReact.ServerManagementService.Controllers;

[ApiController]
[Authorize]
[Route("servers")]
public class ServersController : ControllerBase
{
    private ILogger<ServersController> Logger { get; }
    private IServersService ServersService { get; }

    public ServersController(ILogger<ServersController> logger, IServersService serversService)
    {
        Logger = logger;
        ServersService = serversService;
    }

    [HttpPost("")]
    public async Task<string> CreateServer([FromBody] ServerAddRequest request)
    {
        return await ServersService.CreateServer(request.Name);
    }

    [HttpGet("")]
    public async Task<IEnumerable<ServerDetails>> GetAllServers()
    {
        return await ServersService.GetAllServers();
    }

    [HttpGet("user")]
    public async Task<IEnumerable<ServerDetails>> GetFollowingServers()
    {
        return await ServersService.GetFollowedServers(User.GetUserId());
    }

    [HttpGet("{id}")]
    public async Task<ServerDetails> GetServer(string id)
    {
        return await ServersService.GetServer(id);
    }

    [HttpGet("{serverId}/user")]
    public async Task<bool> GetIsFollowing(string serverId)
    {
        return await ServersService.GetIsFollowing(User.GetUserId(), serverId);
    }
}