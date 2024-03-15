using MauiBlazor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using RESTfulAPI.Gateways;

namespace RESTfulAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ChannelsController : ControllerBase
{
    private ILogger<ServersController> Logger { get; }
    private IChannelsGateway ChannelsGateway { get; }

    public ChannelsController(ILogger<ServersController> logger, IChannelsGateway channelsGateway)
    {
        Logger = logger;
        ChannelsGateway = channelsGateway;
    }

    [HttpGet("[action]")]
    public async Task<IEnumerable<ChannelDetails>> GetChannels([FromQuery] string serverId)
    {
        return await ChannelsGateway.GetByServerId(serverId);
    }

    [HttpGet("[action]")]
    public async Task<ChannelDetails> GetChannel([FromQuery] string id)
    {
        return await ChannelsGateway.GetById(id);
    }
}
