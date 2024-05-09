using Models;

namespace NetReact.MessagingWorker.Services;

public interface IMessagesService
{
    Task<string> Add(string senderId, string channelId, string content, string image);
    Task<ChannelMessage> Get(string messageId);
    Task<IEnumerable<ChannelMessage>> Get(string channelId, int take,  DateTime? from = null);
    Task<bool> Update(string messageId, string newContent);
    Task<bool> Delete(string senderId, string messageId);
}