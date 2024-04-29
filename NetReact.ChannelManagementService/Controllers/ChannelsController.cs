using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using NetReact.ChannelManagementService.Gateways;
using NetReact.ChannelManagementService.Repositories;

namespace NetReact.ChannelManagementService.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ChannelsController : ControllerBase
{
    private ILogger<ChannelsController> Logger { get; }
    private IChannelsGateway ChannelsGateway { get; }

    public ChannelsController(
        ILogger<ChannelsController> logger,
        IChannelsGateway channelsGateway)
    {
        Logger = logger;
        ChannelsGateway = channelsGateway;
    }
    
    [HttpPost("[action]")]
    public async Task<string> CreateChannel([FromBody] ChannelAddRequest request)
    {
        return await ChannelsGateway.CreateServer(request.ServerId, request.Name, request.Type);
    }

    [HttpGet("[action]")]
    public async Task<IEnumerable<ChannelDetails>> GetChannels([FromQuery] string serverId)
    {
        return await ChannelsGateway.GetChannels(serverId);
    }

    [HttpGet("[action]")]
    public async Task<ChannelDetails> GetChannel([FromQuery] string id)
    {
        return await ChannelsGateway.GetChannel(id);
    }
}
