using Microsoft.EntityFrameworkCore;
using Models;
using NetReact.ChannelManagementService.DB;

namespace NetReact.ChannelManagementService.Repositories;

public class ChannelsRepository : IChannelsRepository
{
    private readonly ApplicationContext _dbContext;

    public ChannelsRepository(ApplicationContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<string> Add(ChannelDetails channelDetails)
    {
        ArgumentNullException.ThrowIfNull(channelDetails);

        channelDetails.Id = Guid.NewGuid().ToString();

        await _dbContext.Channels.AddAsync(channelDetails);

        return channelDetails.Id;
    }

    public async Task<ChannelDetails> GetById(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        return await _dbContext.Channels.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<ChannelDetails>> GetByServerId(string serverId)
    {
        ArgumentException.ThrowIfNullOrEmpty(serverId);

        return await _dbContext.Channels.AsNoTracking().Where(x => x.ServerId == serverId).ToArrayAsync();
    }

    public async Task Edit(string id, Action<ChannelDetails> editor)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        ArgumentNullException.ThrowIfNull(editor);

        var server = await _dbContext.Channels.FindAsync(id);
        if (server == null) return;
        editor(server);
    }

    public async Task Delete(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        var server = await _dbContext.Channels.FindAsync(id);
        if (server == null) return;
        _dbContext.Channels.Remove(server);
    }

    public async Task Save()
        => await _dbContext.SaveChangesAsync();
}