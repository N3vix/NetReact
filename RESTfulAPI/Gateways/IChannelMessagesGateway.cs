using Models;

namespace RESTfulAPI.Gateways;

public interface IChannelMessagesGateway
{
    Task<string> Add(ChannelMessage channelDetails);
    Task<IEnumerable<ChannelMessage>> GetByChannelId(string channelId, int take, int skip);
    Task<ChannelMessage> GetById(string id);
    Task<bool> Edit(string id, ChannelMessage message);
    Task<bool> Delete(string id);
}