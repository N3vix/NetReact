using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using RESTfulAPI.Repositories;

namespace RESTfulAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ServersController : ControllerBase
{
    private ILogger<ServersController> Logger { get; }
    private IServersRepository ServersRepository { get; }

    public ServersController(ILogger<ServersController> logger, IServersRepository serversRepository)
    {
        Logger = logger;
        ServersRepository = serversRepository;
    }

    [HttpGet("[action]")]
    public async Task<IEnumerable<ServerDetails>> GetAllServers()
    {
        return await ServersRepository.Get();
    }

    [HttpGet("[action]")]
    public async Task<IEnumerable<ServerDetails>> GetAddedServers()
    {
        var userId = User.Claims.First(c => c.Type == "userid").Value;
        return await ServersRepository.GetByUserId(userId);
    }

    [HttpGet("[action]")]
    public async Task<ServerDetails> GetServer([FromQuery] string id)
    {
        return await ServersRepository.GetById(id);
    }
}
