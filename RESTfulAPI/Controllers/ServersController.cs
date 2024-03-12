using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using RESTfulAPI.Gateways;

namespace MauiBlazor.Services;

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
    public async Task<IEnumerable<ServerDetails>> GetAllServers()
    {
        return await ServersGateway.GetAll();
    }

    [HttpGet("[action]")]
    public async Task<IEnumerable<ServerDetails>> GetAddedServers()
    {
        var userId = User.Claims.First(c => c.Type == "userid").Value;
        return await ServersGateway.GetByUserId(userId);
    }

    [HttpGet("[action]")]
    public async Task<ServerDetails> GetServer([FromQuery] string id)
    {
        return await ServersGateway.GetByServerId(id);
    }
}
