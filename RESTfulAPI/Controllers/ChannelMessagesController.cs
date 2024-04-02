using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using RESTfulAPI.Gateways;

namespace RESTfulAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ChannelMessagesController : ControllerBase
{
    private ILogger<ChannelMessagesController> Logger { get; }
    private IChannelMessagesGateway MessagesGateway { get; }
    private IMessageMediaGetaway MessageMediaGetaway { get; }

    public ChannelMessagesController(
        ILogger<ChannelMessagesController> logger,
        IChannelMessagesGateway messagesGateway,
        IMessageMediaGetaway messageMediaGetaway)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(messagesGateway);

        Logger = logger;
        MessagesGateway = messagesGateway;
        MessageMediaGetaway = messageMediaGetaway;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Add([FromForm] ChannelMessageAddRequest request)
    {
        var userId = User.Claims.First(c => c.Type == "userid").Value;

        var fileName = await MessageMediaGetaway.WriteAsync(request.Image);
        var addedMessageId = await MessagesGateway.Add(userId, request.ChannelId, request.Content, fileName);
        return Ok(new { messageId = addedMessageId });
    }

    [HttpPost("[action]")]
    public async Task<ChannelMessage> GetById([FromBody] ChannelMessageGetByIdRequest request)
    {
        return await MessagesGateway.Get(request.MessageId);
    }

    [HttpPost("[action]")]
    public async Task<IEnumerable<ChannelMessage>> Get([FromBody] ChannelMessageGetRequest request)
    {
        return await MessagesGateway.Get(request.ChannelId, request.Take);
    }

    [HttpPost("[action]")]
    public async Task<IEnumerable<ChannelMessage>> GetBefore([FromBody] ChannelMessageGetRequest request)
    {
        return await MessagesGateway.GetBefore(request.DateTime, request.ChannelId, request.Take);
    }

    [HttpPost("[action]")]
    public async Task<bool> Update([FromBody] ChannelMessageUpdateRequest request)
    {
        var userId = User.Claims.First(c => c.Type == "userid").Value;

        return await MessagesGateway.Update(userId, request.MessageId, request.Content);
    }

    [HttpPost("[action]")]
    public async Task<bool> Delete([FromBody] ChannelMessageDeleteRequest request)
    {
        var userId = User.Claims.First(c => c.Type == "userid").Value;

        return await MessagesGateway.Delete(userId, request.MessageId);
    }
}