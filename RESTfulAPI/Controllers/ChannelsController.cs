using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using RESTfulAPI.Repositories;

namespace RESTfulAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ChannelsController : ControllerBase
{
    private ILogger<ServersController> Logger { get; }
    private IChannelsRepository ChannelsRepository { get; }

    public ChannelsController(ILogger<ServersController> logger, IChannelsRepository channelsRepository)
    {
        Logger = logger;
        ChannelsRepository = channelsRepository;
    }

    [HttpGet("[action]")]
    public async Task<IEnumerable<ChannelDetails>> GetChannels([FromQuery] string serverId)
    {
        return await ChannelsRepository.GetByServerId(serverId);
    }

    [HttpGet("[action]")]
    public async Task<ChannelDetails> GetChannel([FromQuery] string id)
    {
        return await ChannelsRepository.GetById(id);
    }
}
