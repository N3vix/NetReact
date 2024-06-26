using Models;
using NetReact.MessagingWorker.Repositories;
using OpenTelemetry.Trace;

namespace NetReact.MessagingWorker.Services;

public class MessagesService : IMessagesService
{
    private readonly Tracer _tracer;
    private ILogger<MessagesService> Logger { get; }
    private IMessagesRepository MessagesRepository { get; }

    public MessagesService(
        Tracer tracer,
        ILogger<MessagesService> logger,
        IMessagesRepository messagesRepository)
    {
        _tracer = tracer;
        Logger = logger;
        MessagesRepository = messagesRepository;
    }

    public async Task<string> Add(string senderId, string channelId, string content, string image)
    {
        using var _ = _tracer.StartSpan(nameof(Add));
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
        using var _ = _tracer.StartSpan(nameof(Get));
        return await MessagesRepository.GetById(messageId);
    }

    public async Task<IEnumerable<ChannelMessage>> Get(string channelId, int take, DateTime? from = null)
    {
        using var _ = _tracer.StartSpan(nameof(Get));
        return await MessagesRepository.Get(channelId, take, from);
    }

    public async Task<bool> Update(string messageId, string newContent)
    {
        using var _ = _tracer.StartSpan(nameof(Update));
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
        using var _ = _tracer.StartSpan(nameof(Delete));
        var result = await MessagesRepository.Delete(messageId);
        if (result)
            await MessagesRepository.Save();
        return result;
    }
}