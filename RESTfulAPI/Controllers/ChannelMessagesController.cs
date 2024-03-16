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

    public ChannelMessagesController(ILogger<ChannelMessagesController> logger, IChannelMessagesGateway messagesGateway)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(messagesGateway);

        Logger = logger;
        MessagesGateway = messagesGateway;
    }

    [HttpPost("[action]")]
    public async Task<string> Add([FromBody] ChannelMessageAddRequest request)
    {
        var userId = User.Claims.First(c => c.Type == "userid").Value;

        return await MessagesGateway.Add(userId, request.ChannelId, request.Content);
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
}