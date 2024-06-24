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
            Content = content,
            Image = image
        };

        var messageId = await MessagesRepository.Add(channelMessage);
        await MessagesRepository.Save();
        
        return messageId;
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
        var result = await MessagesRepository.Edit(messageId, channelMessage =>
        {
            channelMessage.Content = newContent;
            channelMessage.EditedTimestamp = DateTime.Now;
        });
        if (result)
            await MessagesRepository.Save();
        return result;
    }

    public async Task<bool> Delete(string messageId)
    {
        var result = await MessagesRepository.Delete(messageId);
        if (result)
            await MessagesRepository.Save();
        return result;
    }
}