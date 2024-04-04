using Models;

namespace RESTfulAPI.Gateways;

public interface IChannelMessagesGateway
{
    Task<Result<string>> Add(string senderId, string channelId, string content, string image);
    Task<Result<bool>> Delete(string senderId, string messageId);
    Task<Result<ChannelMessage>> Get(string senderId, string messageId);
    Task<Result<IEnumerable<ChannelMessage>>> Get(string senderId, string channelId, int take);
    Task<Result<IEnumerable<ChannelMessage>>> GetBefore(string senderId, string channelId, DateTime dateTime, int take);
    Task<Result<bool>> Update(string senderId, string messageId, string newContent);
}