using Models;

namespace RESTfulAPI.Repositories;

public interface IChannelsRepository
{
    Task<string> Add(ChannelDetails channelDetails);
    Task<ChannelDetails> GetById(string id);
    Task<IEnumerable<ChannelDetails>> GetByServerId(string serverId);
    Task<bool> Edit(string id, ChannelDetails channelDetails);
    Task<bool> Delete(string id);
}