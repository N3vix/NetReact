using Models;

namespace NetReact.MessagingService.Gateways;

public interface IMessagesGateway
{
    Task<string> Add(string senderId, string channelId, string content, string image);
    Task<bool> Delete(string senderId, string messageId);
    Task<ChannelMessage> Get(string senderId, string messageId);
    Task<IEnumerable<ChannelMessage>> Get(string senderId, string channelId, int take);
    Task<IEnumerable<ChannelMessage>> GetBefore(string senderId, string channelId, DateTime dateTime, int take);
    Task<bool> Update(string senderId, string messageId, string newContent);
}