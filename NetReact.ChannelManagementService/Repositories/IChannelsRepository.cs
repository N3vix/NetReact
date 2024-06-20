using Models;

namespace NetReact.ChannelManagementService.Repositories;

public interface IChannelsRepository
{
    Task<string> Add(ChannelDetails channelDetails);
    Task<ChannelDetails> GetById(string id);
    Task<IEnumerable<ChannelDetails>> GetByServerId(string serverId);
    Task Edit(string id, Action<ChannelDetails> editor);
    Task Delete(string id);
    Task Save();
}