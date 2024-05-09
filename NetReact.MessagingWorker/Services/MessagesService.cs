using Models;
using NetReact.MessagingWorker.Repositories;

namespace NetReact.MessagingWorker.Services;

public class MessagesService : IMessagesService
{
    private ILogger<MessagesService> Logger { get; }
    private IMessagesRepository MessagesRepository { get; }

    public MessagesService(
        ILogger<MessagesService> logger,
        IMessagesRepository messagesRepository)
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

    public async Task<ChannelMessage> Get(string messageId)
    {
        return await MessagesRepository.GetById(messageId);
    }

    public async Task<IEnumerable<ChannelMessage>> Get(string channelId, int take, DateTime? from = null)
    {
        return await MessagesRepository.Get(channelId, take, from);
    }

    public async Task<bool> Update(string messageId, string newContent)
    {
        var message = await Get(messageId);
        message.Content = newContent;
        message.EditedTimestamp = DateTime.UtcNow;
        return await MessagesRepository.Edit(messageId, message);
    }

    public async Task<bool> Delete(string senderId, string messageId)
    {
        var message = await Get(messageId);

        if (!senderId.Equals(message.SenderId)) return false;
        return await MessagesRepository.Delete(messageId);
    }
}