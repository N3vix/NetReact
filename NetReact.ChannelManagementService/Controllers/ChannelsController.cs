using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using NetReact.ChannelManagementService.Gateways;

namespace NetReact.ChannelManagementService.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ChannelsController : ControllerBase
{
    private readonly ILogger<ChannelsController> _logger;
    private readonly ChannelServiceHttpClient _httpClient;
    private readonly IChannelsGateway _channelsGateway;

    public ChannelsController(
        ILogger<ChannelsController> logger,
        ChannelServiceHttpClient httpClient,
        IChannelsGateway channelsGateway)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(channelsGateway);

        _logger = logger;
        _httpClient = httpClient;
        _channelsGateway = channelsGateway;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateChannel([FromBody] ChannelAddRequest request)
    {
        var userId = User.GetUserId();
        var response = await _httpClient.GetIsFollowingServer(userId, request.ServerId);
        if (!response.IsSuccessStatusCode)
            return BadRequest(new { Error = "Server management service failed." });

        var content = await response.Content.ReadAsStringAsync();
        if (!bool.TryParse(content, out var result) || !result)
            return BadRequest(new { Error = "Operation not allowed." });

        var channelId = await _channelsGateway.CreateServer(request.ServerId, request.Name, request.Type);
        return Ok(channelId);
    }

    [HttpGet("[action]")]
    public async Task<IEnumerable<ChannelDetails>> GetChannels([FromQuery] string serverId)
    {
        return await _channelsGateway.GetChannels(serverId);
    }

    [HttpGet("[action]")]
    public async Task<ChannelDetails> GetChannel([FromQuery] string id)
    {
        return await _channelsGateway.GetChannel(id);
    }
}