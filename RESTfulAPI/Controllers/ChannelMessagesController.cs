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

    private string ImagesPath { get; }

    public ChannelMessagesController(ILogger<ChannelMessagesController> logger, IChannelMessagesGateway messagesGateway)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(messagesGateway);

        Logger = logger;
        MessagesGateway = messagesGateway;

        ImagesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "DbImages");
    }

    [HttpPost("[action]")]
    public async Task<string> Add([FromForm] ChannelMessageAddRequest request)
    {
        var userId = User.Claims.First(c => c.Type == "userid").Value;

        if (request.Image != null)
        {
            var newImagePath = Path.Combine(ImagesPath, request.Image.FileName);
            using var fileStream = new FileStream(newImagePath, FileMode.Create);
            await request.Image.CopyToAsync(fileStream);
        }

        return await MessagesGateway.Add(userId, request.ChannelId, request.Content, request.Image?.FileName);
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
}