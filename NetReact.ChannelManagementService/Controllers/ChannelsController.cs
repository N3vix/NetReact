using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using NetReact.ChannelManagementService.Repositories;

namespace NetReact.ChannelManagementService.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ChannelsController : ControllerBase
{
    private ILogger<ChannelsController> Logger { get; }
    private IChannelsRepository ChannelsRepository { get; }

    public ChannelsController(ILogger<ChannelsController> logger, IChannelsRepository channelsRepository)
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
