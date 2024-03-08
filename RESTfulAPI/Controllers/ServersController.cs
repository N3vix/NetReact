using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using RESTfulAPI;

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
    public async Task<IEnumerable<ServerDetails>> GetServers([FromQuery] string idsCsv = null)
    {
        if (string.IsNullOrEmpty(idsCsv)) return await ServersGateway.GetAll();
        var ids = idsCsv.Split(',');
        return await ServersGateway.GetByServerId(ids);
    }

    [HttpGet("[action]")]
    public async Task<ServerDetails> GetServer([FromQuery] string id)
    {
        return await ServersGateway.GetByServerId(id);
    }
}
