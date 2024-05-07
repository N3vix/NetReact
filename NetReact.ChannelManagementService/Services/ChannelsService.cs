using Models;
using NetReact.ChannelManagementService.Repositories;

namespace NetReact.ChannelManagementService.Services;

internal class ChannelsService : IChannelsService
{
    private IChannelsRepository ChannelsRepository { get; }

    public ChannelsService(IChannelsRepository channelsRepository)
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