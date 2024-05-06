using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using NetReact.MessageBroker;
using NetReact.MessageBroker.SharedModels;
using NetReact.MessagingService.Gateways;

namespace NetReact.MessagingService.Controllers;

[ApiController]
[Authorize]
[Route("channel")]
public class MessagesController : ControllerBase
{
    private readonly IMessageBrokerProducer _messageProducer;

    private ILogger<MessagesController> Logger { get; }
    private MessagesServiceHttpClient HttpClient { get; }
    private IMessagesGateway MessagesGateway { get; }
    private IMessageMediaGetaway MessageMediaGetaway { get; }

    public MessagesController(
        ILogger<MessagesController> logger,
        MessagesServiceHttpClient httpClient,
        IMessagesGateway messagesGateway,
        IMessageMediaGetaway messageMediaGetaway,
        IMessageBrokerProducer messageProducer)
    {
        _messageProducer = messageProducer;
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(messageMediaGetaway);
        ArgumentNullException.ThrowIfNull(messageProducer);

        Logger = logger;
        HttpClient = httpClient;
        MessagesGateway = messagesGateway;
        MessageMediaGetaway = messageMediaGetaway;
    }

    [HttpPost("messages")]
    public async Task<IActionResult> Add([FromForm] ChannelMessageAddRequest request)
    {
        var userId = User.GetUserId();

        var isFollowing = await IsFollowing(userId, request.ChannelId);
        if (isFollowing.IsError)
            return BadRequest(new { Error = isFollowing.Error });

        var messageCreated = new ChannelMessageCreated
        {
            SenderId = userId,
            ChannelId = request.ChannelId,
            Content = request.Content,
            Image = GetByteArray(request.Image)
        };
        _messageProducer.SendMessage(messageCreated);

        var fileName = await MessageMediaGetaway.WriteAsync(request.Image);
        var addedMessageId = await MessagesGateway.Add(userId, request.ChannelId, request.Content, fileName);

        return Ok(addedMessageId);
    }

    [HttpGet("{channelId}/messages/{messageId}")]
    public async Task<IActionResult> GetById(string channelId, string messageId)
    {
        var userId = User.GetUserId();

        var isFollowing = await IsFollowing(userId, channelId);
        if (isFollowing.IsError)
            return BadRequest(new { Error = isFollowing.Error });

        var result = await MessagesGateway.Get(messageId);

        return Ok(result);
    }

    [HttpGet("{channelId}/messages")]
    public async Task<IActionResult> Get(string channelId, [FromQuery] int take, [FromQuery] DateTime? from = null)
    {
        var userId = User.GetUserId();

        var isFollowing = await IsFollowing(userId, channelId);
        if (isFollowing.IsError)
            return BadRequest(new { Error = isFollowing.Error });

        var result = await MessagesGateway.Get(channelId, take, from);

        return Ok(result);
    }

    [HttpPut("messages")]
    public async Task<IActionResult> Update([FromBody] ChannelMessageUpdateRequest request)
    {
        var userId = User.GetUserId();

        var isFollowing = await IsFollowing(userId, request.ChannelId);
        if (isFollowing.IsError)
            return BadRequest(new { Error = isFollowing.Error });

        var result = await MessagesGateway.Update(User.GetUserId(), request.MessageId, request.Content);

        return Ok(result);
    }

    [HttpDelete("messages")]
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

    private byte[]? GetByteArray(IFormFile? formFile)
    {
        if (formFile == null)
            return null;
        using var stream = new MemoryStream();
        formFile.CopyTo(stream);
        return stream.ToArray();
    }
}