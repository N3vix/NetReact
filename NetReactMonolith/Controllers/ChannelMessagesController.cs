using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using NetReactMonolith.Gateways;

namespace NetReactMonolith.Controllers;

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

        return UnpuckResult(addedMessageId, value => new { messageId = value });
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> GetById([FromBody] ChannelMessageGetByIdRequest request)
    {
        var result = await MessagesGateway.Get(User.GetUserId(), request.MessageId);

        return UnpuckResult(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Get([FromBody] ChannelMessageGetRequest request)
    {
        var result = await MessagesGateway.Get(User.GetUserId(), request.ChannelId, request.Take);

        return UnpuckResult(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> GetBefore([FromBody] ChannelMessageGetRequest request)
    {
        var result = await MessagesGateway.GetBefore(User.GetUserId(), request.ChannelId, request.DateTime, request.Take);

        return UnpuckResult(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Update([FromBody] ChannelMessageUpdateRequest request)
    {
        var result = await MessagesGateway.Update(User.GetUserId(), request.MessageId, request.Content);

        return UnpuckResult(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Delete([FromBody] ChannelMessageDeleteRequest request)
    {
        var result = await MessagesGateway.Delete(User.GetUserId(), request.MessageId);

        return UnpuckResult(result);
    }

    private IActionResult UnpuckResult<TValue, TError>(Result<TValue, TError> result, Func<TValue, object> valueBuilder = null)
    {
        if (result.IsSuccess)
            return Ok(valueBuilder == null ? result.Value : valueBuilder(result.Value));

        return BadRequest(new { Error = result.Error });
    }
}