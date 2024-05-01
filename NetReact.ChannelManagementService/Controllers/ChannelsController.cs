using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using NetReact.ChannelManagementService.Gateways;

namespace NetReact.ChannelManagementService.Controllers;

[ApiController]
[Authorize]
[Route("channels")]
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

    [HttpPost("")]
    public async Task<IActionResult> CreateChannel([FromBody] ChannelAddRequest request)
    {
        var isFollowing = await IsFollowing(request.ServerId);
        if (isFollowing.IsError)
            return BadRequest(new { Error = isFollowing.Error });

        var channelId = await _channelsGateway.CreateServer(request.ServerId, request.Name, request.Type);
        return Ok(channelId);
    }

    [HttpGet("{serverId}/all")]
    public async Task<IEnumerable<ChannelDetails>> GetChannels(string serverId)
    {
        return await _channelsGateway.GetChannels(serverId);
    }

    [HttpGet("{id}")]
    public async Task<ChannelDetails> GetChannel(string id)
    {
        return await _channelsGateway.GetChannel(id);
    }

    [HttpGet("{channelId}/user")]
    public async Task<IActionResult> GetIsFollowing(string channelId)
    {
        var channelDetails = await _channelsGateway.GetChannel(channelId);
        if (channelDetails == null) return NotFound("Channel not found.");

        var isFollowing = await IsFollowing(channelDetails.ServerId);
        if (isFollowing.IsError)
            return BadRequest(new { Error = isFollowing.Error });

        return Ok(true);
    }

    private async Task<Result<bool, string>> IsFollowing(string serverId)
    {
        var response = await _httpClient.GetIsFollowingServer(serverId);
        if (!response.IsSuccessStatusCode)
            return "Server management service failed.";

        var content = await response.Content.ReadAsStringAsync();
        if (!bool.TryParse(content, out var result) || !result)
            return "Operation not allowed.";

        return true;
    }
}