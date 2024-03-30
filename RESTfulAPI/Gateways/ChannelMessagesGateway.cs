using Models;
using RESTfulAPI.Repositories;

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

    public async Task<bool> Update(string senderId, string messageId, string newContent)
    {
        var message = await Get(messageId);
        if (!senderId.Equals(message.SenderId)) return false;
        message.Content = newContent;
        return await MessagesRepository.Edit(messageId, message);
    }

    public async Task<bool> Delete(string senderId, string messageId)
    {
        var message = await Get(messageId);
        if (!senderId.Equals(message.SenderId)) return false;
        return await MessagesRepository.Delete(messageId);
    }
}
