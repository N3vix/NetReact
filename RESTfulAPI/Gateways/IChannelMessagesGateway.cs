using Models;

namespace RESTfulAPI.Gateways;

public interface IChannelMessagesGateway
{
    Task<Result<string, string>> Add(string senderId, string channelId, string content, string image);
    Task<Result<bool, string>> Delete(string senderId, string messageId);
    Task<Result<ChannelMessage, string>> Get(string senderId, string messageId);
    Task<Result<IEnumerable<ChannelMessage>, string>> Get(string senderId, string channelId, int take);
    Task<Result<IEnumerable<ChannelMessage>, string>> GetBefore(string senderId, string channelId, DateTime dateTime, int take);
    Task<Result<bool, string>> Update(string senderId, string messageId, string newContent);
}