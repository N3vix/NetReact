using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using NetReact.MessagingService.Gateways;

namespace NetReact.MessagingService.Controllers;

[ApiController]
[Authorize]
[Route("channel")]
public class MessagesController : ControllerBase
{
    private readonly IMessagesGateway _messagesGateway;

    public MessagesController(IMessagesGateway messagesGateway)
    {
        ArgumentNullException.ThrowIfNull(messagesGateway);

        _messagesGateway = messagesGateway;
    }

    [HttpPost("messages")]
    public async Task<IActionResult> Add([FromForm] ChannelMessageAddRequest request)
    {
        var userId = User.GetUserId();
        var image = GetByteArray(request.Image);

        var result = await _messagesGateway.Add(userId, request.ChannelId, request.Content, image);
        return UnpuckResult(result);
    }

    [HttpGet("{channelId}/messages/{messageId}")]
    public async Task<IActionResult> GetById(string channelId, string messageId)
    {
        var userId = User.GetUserId();

        var result = await _messagesGateway.Get(userId, channelId, messageId);
        return UnpuckResult(result);
    }

    [HttpGet("{channelId}/messages")]
    public async Task<IActionResult> Get(string channelId, [FromQuery] int take, [FromQuery] DateTime? from = null)
    {
        var userId = User.GetUserId();

        var result = await _messagesGateway.Get(userId, channelId, take, from);
        return UnpuckResult(result);
    }

    [HttpPut("messages")]
    public async Task<IActionResult> Update([FromBody] ChannelMessageUpdateRequest request)
    {
        var userId = User.GetUserId();

        var result = await _messagesGateway.Update(userId, request.ChannelId, request.MessageId, request.Content);
        return UnpuckResult(result);
    }

    [HttpDelete("messages")]
    public async Task<IActionResult> Delete([FromBody] ChannelMessageDeleteRequest request)
    {
        var userId = User.GetUserId();

        var result = await _messagesGateway.Delete(userId, request.ChannelId, request.MessageId);
        return UnpuckResult(result);
    }

    private byte[]? GetByteArray(IFormFile? formFile)
    {
        if (formFile == null)
            return null;
        using var stream = new MemoryStream();
        formFile.CopyTo(stream);
        return stream.ToArray();
    }

    private IActionResult UnpuckResult<TError>(Result<TError> result)
    {
        if (result.IsSuccess) return Ok();
        return BadRequest(new { Error = result.Error });
    }

    private IActionResult UnpuckResult<TValue, TError>(
        Result<TValue, TError> result,
        Func<TValue, object> valueBuilder = null)
    {
        if (result.IsSuccess)
            return Ok(valueBuilder == null ? result.Value : valueBuilder(result.Value));

        return BadRequest(new { Error = result.Error });
    }
}