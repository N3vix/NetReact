using Models;

namespace RESTfulAPI.Gateways;

public interface IChannelsGateway
{
    Task<string> Add(ChannelDetails channelDetails);
    Task<ChannelDetails> GetById(string id);
    Task<IEnumerable<ChannelDetails>> GetByServerId(string serverId);
    Task<bool> Edit(string id, ChannelDetails channelDetails);
    Task<bool> Delete(string id);
}