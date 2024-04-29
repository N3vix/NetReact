using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using NetReact.MessagingService.Gateways;

namespace NetReact.MessagingService.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ChannelMessagesController : ControllerBase
{
    private const string UserNotFollowingError = "The user does not follow the specified server";

    private ILogger<ChannelMessagesController> Logger { get; }
    private MessagesServiceHttpClient HttpClient { get; }
    private IMessagesGateway MessagesGateway { get; }
    private IMessageMediaGetaway MessageMediaGetaway { get; }

    public ChannelMessagesController(
        ILogger<ChannelMessagesController> logger,
        MessagesServiceHttpClient httpClient,
        IMessagesGateway messagesGateway,
        IMessageMediaGetaway messageMediaGetaway)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(messageMediaGetaway);

        Logger = logger;
        HttpClient = httpClient;
        MessagesGateway = messagesGateway;
        MessageMediaGetaway = messageMediaGetaway;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Add([FromForm] ChannelMessageAddRequest request)
    {
        var userId = User.GetUserId();

        var isFollowing = await IsFollowing(userId, request.ChannelId);
        if (isFollowing.IsError)
            return BadRequest(new { Error = isFollowing.Error });

        var fileName = await MessageMediaGetaway.WriteAsync(request.Image);
        var addedMessageId = await MessagesGateway.Add(userId, request.ChannelId, request.Content, fileName);

        return Ok(addedMessageId);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> GetById([FromBody] ChannelMessageGetByIdRequest request)
    {
        var userId = User.GetUserId();

        var isFollowing = await IsFollowing(userId, request.ChannelId);
        if (isFollowing.IsError)
            return BadRequest(new { Error = isFollowing.Error });
        
        var result = await MessagesGateway.Get(User.GetUserId(), request.MessageId);

        return Ok(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Get([FromBody] ChannelMessageGetRequest request)
    {
        var userId = User.GetUserId();

        var isFollowing = await IsFollowing(userId, request.ChannelId);
        if (isFollowing.IsError)
            return BadRequest(new { Error = isFollowing.Error });
        
        var result = await MessagesGateway.Get(User.GetUserId(), request.ChannelId, request.Take);

        return Ok(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> GetBefore([FromBody] ChannelMessageGetRequest request)
    {
        var userId = User.GetUserId();

        var isFollowing = await IsFollowing(userId, request.ChannelId);
        if (isFollowing.IsError)
            return BadRequest(new { Error = isFollowing.Error });
        
        var result =
            await MessagesGateway.GetBefore(User.GetUserId(), request.ChannelId, request.DateTime, request.Take);

        return Ok(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Update([FromBody] ChannelMessageUpdateRequest request)
    {
        var userId = User.GetUserId();

        var isFollowing = await IsFollowing(userId, request.ChannelId);
        if (isFollowing.IsError)
            return BadRequest(new { Error = isFollowing.Error });
        
        var result = await MessagesGateway.Update(User.GetUserId(), request.MessageId, request.Content);

        return Ok(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Delete([FromBody] ChannelMessageDeleteRequest request)
    {        
        var userId = User.GetUserId();

        var isFollowing = await IsFollowing(userId, request.ChannelId);
        if (isFollowing.IsError)
            return BadRequest(new { Error = isFollowing.Error });
        
        var result = await MessagesGateway.Delete(User.GetUserId(), request.MessageId);

        return Ok(result);
    }    
    
    private async Task<Result<bool, string>> IsFollowing(string userId, string channelId)
    {
        var response = await HttpClient.GetIsFollowingServer(userId, channelId);
        if (!response.IsSuccessStatusCode)
            return "Channel management service failed.";

        var content = await response.Content.ReadAsStringAsync();
        if (!bool.TryParse(content, out var result) || !result)
            return "Operation not allowed.";

        return true;
    }
}