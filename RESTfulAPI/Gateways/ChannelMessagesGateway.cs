using Models;
using RESTfulAPI.Repositories;
using System.Threading.Channels;

namespace RESTfulAPI.Gateways;

public class ChannelMessagesGateway : IChannelMessagesGateway
{
    private ILogger<ChannelMessagesGateway> Logger { get; }
    private IChannelMessagesRepository MessagesRepository { get; }

    public ChannelMessagesGateway(
        ILogger<ChannelMessagesGateway> logger,
        IChannelMessagesRepository messagesRepository)
    {
        Logger = logger;
        MessagesRepository = messagesRepository;
    }

    public async Task<string> Add(string senderId, string channelId, string content, string image)
    {
        var channelMessage = new ChannelMessage
        {
            ChannelId = channelId,
            SenderId = senderId,
            Timestamp = DateTime.UtcNow,
            Content = content,
            Image = image
        };

        return await MessagesRepository.Add(channelMessage);
    }

    public async Task<ChannelMessage> Get(string id)
    {
        return await MessagesRepository.GetById(id);
    }

    public async Task<IEnumerable<ChannelMessage>> Get(string channelId, int take)
    {
        return await MessagesRepository.Get(channelId, take, 0);
    }

    public async Task<IEnumerable<ChannelMessage>> GetBefore(DateTime dateTime, string channelId, int take)
    {
        return await MessagesRepository.GetBefore(dateTime, channelId, take);
    }
}
