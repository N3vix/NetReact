using Models;

namespace NetReact.ChannelManagementService.Services;

public interface IChannelsService
{
    Task<Result<string, string>> CreateServer(string serverId, string name, ChannelType type);
    Task<Result<IEnumerable<ChannelDetails>, string>> GetChannels(string serverId);
    Task<Result<ChannelDetails, string>> GetChannel(string id);
    Task<Result<bool, string>> GetIsFollowing(string channelId);
}