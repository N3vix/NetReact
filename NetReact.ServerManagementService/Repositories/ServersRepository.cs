using Microsoft.EntityFrameworkCore;
using Models;
using NetReact.ServerManagementService.DB;

namespace NetReact.ServerManagementService.Repositories;

public class ServersRepository : IServersRepository
{
    private readonly ApplicationContext _dbContext;

    public ServersRepository(ApplicationContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<string> Add(ServerDetails serverDetails)
    {
        ArgumentNullException.ThrowIfNull(serverDetails);

        serverDetails.Id = Guid.NewGuid().ToString();

        await _dbContext.Servers.AddAsync(serverDetails);

        return serverDetails.Id;
    }

    public async Task Add(params ServerDetails[] serverDetails)
    {
        ArgumentNullException.ThrowIfNull(serverDetails);

        foreach (var server in serverDetails)
        {
            await Add(server);
        }
    }

    public async Task<IEnumerable<ServerDetails>> Get()
    {
        //TODO IMPLEMENT PAGINATION
        return await _dbContext.Servers.AsNoTracking().ToArrayAsync();
    }

    public async Task<ServerDetails> GetById(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        return await _dbContext.Servers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<ServerDetails>> GetByUserId(string userId)
    {
        ArgumentException.ThrowIfNullOrEmpty(userId);

        var servers =
            from s in _dbContext.Servers.AsNoTracking()
            join f in _dbContext.Followers.AsNoTracking()
                on s.Id equals f.ServerId
            where f.UserId == userId
            select s;

        return await servers.ToArrayAsync();
    }

    public async Task<bool> GetIsUserFollowingServer(string userId, string serverId)
    {
        ArgumentException.ThrowIfNullOrEmpty(userId);
        ArgumentException.ThrowIfNullOrEmpty(serverId);

        var servers =
            from s in _dbContext.Servers.AsNoTracking()
            join f in _dbContext.Followers.AsNoTracking()
                on s.Id equals f.ServerId
            where f.ServerId == serverId && f.UserId == userId
            select s;

        return await servers.AnyAsync();
    }

    public async Task Edit(string id, Action<ServerDetails> editor)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        ArgumentNullException.ThrowIfNull(editor);

        var server = await _dbContext.Servers.FindAsync(id);
        if (server == null) return;
        editor(server);
    }

    public async Task Delete(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        var server = await _dbContext.Servers.FindAsync(id);
        if (server == null) return;
        _dbContext.Servers.Remove(server);
    }

    public async Task Save()
        => await _dbContext.SaveChangesAsync();
}