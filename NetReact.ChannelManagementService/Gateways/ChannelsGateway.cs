using Models;
using NetReact.ChannelManagementService.Repositories;

namespace NetReact.ChannelManagementService.Gateways;

internal class ChannelsGateway : IChannelsGateway
{
    private IChannelsRepository ChannelsRepository { get; }

    public ChannelsGateway(IChannelsRepository channelsRepository)
    {
        ChannelsRepository = channelsRepository;
    }

    public async Task<string> CreateServer(string serverId, string name, ChannelType type)
    {
        return await ChannelsRepository.Add(new ChannelDetails
        {
            Id = Guid.NewGuid().ToString(),
            ServerId = serverId,
            Name = name,
            Type = type
        });
    }

    public async Task<IEnumerable<ChannelDetails>> GetChannels(string serverId)
    {
        return await ChannelsRepository.GetByServerId(serverId);
    }

    public async Task<ChannelDetails> GetChannel(string id)
    {
        return await ChannelsRepository.GetById(id);
    }
}