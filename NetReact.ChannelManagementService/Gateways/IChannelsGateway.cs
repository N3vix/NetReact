using Models;

namespace NetReact.ChannelManagementService.Gateways;

public interface IChannelsGateway
{
    Task<string> CreateServer(string serverId, string name, ChannelType type);
    Task<IEnumerable<ChannelDetails>> GetChannels(string serverId);
    Task<ChannelDetails> GetChannel(string id);
}