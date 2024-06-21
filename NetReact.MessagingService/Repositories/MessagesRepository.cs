using Microsoft.EntityFrameworkCore;
using Models;
using NetReact.MessagingService.DB;

namespace NetReact.MessagingService.Repositories;

public class MessagesRepository : IMessagesRepository
{
    private readonly ApplicationContext _dbContext;

    public MessagesRepository(ApplicationContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<string> Add(ChannelMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        message.Id = Ulid.NewUlid().ToString();

        await _dbContext.Messages.AddAsync(message);

        return message.Id;
    }

    public async Task<ChannelMessage> GetById(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        return await _dbContext.Messages.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<ChannelMessage>> Get(string channelId, int take, DateTime? from = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(channelId);
        
        var messages = _dbContext.Messages.Where(x => x.ChannelId == channelId);
        if (from != null)
            messages = messages.Where(x => x.Timestamp < from);
        return await messages
            .OrderByDescending(x => x.Timestamp)
            .Take(take)
            .ToArrayAsync();
    }

    public async Task Edit(string id, Action<ChannelMessage> editor)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        ArgumentNullException.ThrowIfNull(editor);

        var server = await _dbContext.Messages.FindAsync(id);
        if (server == null) return;
        editor(server);
    }

    public async Task Delete(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        var server = await _dbContext.Messages.FindAsync(id);
        if (server == null) return;
        _dbContext.Messages.Remove(server);
    }

    public async Task Save()
        => await _dbContext.SaveChangesAsync();
}
