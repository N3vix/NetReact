using MauiBlazor.Services;
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
    private ILogger<ServersController> Logger { get; }
    private IChannelMessagesGateway MessagesGateway { get; }

    public ChannelMessagesController(ILogger<ServersController> logger, IChannelMessagesGateway messagesGateway)
    {
        Logger = logger;
        MessagesGateway = messagesGateway;
    }

    [HttpGet("[action]")]
    public async Task<string> AddMessage([FromBody] ChannelMessageRequest channelMessageRequest)
    {
        var channelMessage = new ChannelMessage
        {
            ChannelId = channelMessageRequest.ChannelId,
            SenderId = User.Claims.First(c => c.Type == "userid").Value,
            Timestamp = DateTime.UtcNow,
            Content = channelMessageRequest.Content
        };

        return await MessagesGateway.Add(channelMessage);
    }

    [HttpGet("[action]")]
    public async Task<IEnumerable<ChannelMessage>> GetMessages([FromQuery] string id, [FromQuery] int take)
    {
        return await MessagesGateway.GetByChannelId(id, take, 0);
    }
}