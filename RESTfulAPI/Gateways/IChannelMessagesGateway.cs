using Models;

namespace RESTfulAPI.Gateways;

public interface IChannelMessagesGateway
{
    Task<string> Add(string senderId, string channelId, string content);
    Task<ChannelMessage> Get(string id);
    Task<IEnumerable<ChannelMessage>> Get(string channelId, int take);
    Task<IEnumerable<ChannelMessage>> GetBefore(DateTime dateTime, string channelId, int take);
}