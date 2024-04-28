using System.Linq.Expressions;
using Models;
using MongoDB.Driver;
using NetReact.ServerManagementService.DB;

namespace NetReact.ServerManagementService.Repositories;

public class ServerFollowersRepository : IServerFollowersRepository
{
    private readonly IMongoDbContext _mongoDbContext;

    public ServerFollowersRepository(IMongoDbContext mongoDbContext)
    {
        ArgumentNullException.ThrowIfNull(mongoDbContext);

        _mongoDbContext = mongoDbContext;
    }

    public async Task<string> Add(ServerFollower follower)
    {
        ArgumentNullException.ThrowIfNull(follower);

        follower.Id = Guid.NewGuid().ToString();

        await _mongoDbContext.Followers.InsertOneAsync(follower);

        return follower.Id;
    }

    public async Task<IEnumerable<ServerFollower>> GetFollowedServers(string userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var filter = GetFilter(x => x.UserId, userId);

        var result = await _mongoDbContext.Followers.Find(filter).ToListAsync();

        return result;
    }

    public async Task<IEnumerable<ServerFollower>> GetFollowedUsers(string serverId)
    {
        ArgumentNullException.ThrowIfNull(serverId);

        var filter = GetFilter(x => x.ServerId, serverId);

        var result = await _mongoDbContext.Followers.Find(filter).ToListAsync();

        return result;
    }

    public async Task<bool> GetIsUserFollowingServer(string userId, string serverId)
    {
        ArgumentException.ThrowIfNullOrEmpty(userId);
        ArgumentException.ThrowIfNullOrEmpty(serverId);

        var filter =
            GetFilter(x => x.UserId, userId) &
            GetFilter(x => x.ServerId, serverId);

        var result = await _mongoDbContext.Followers.Find(filter)?.FirstOrDefaultAsync();

        return result != null;
    }

    public async Task<bool> Delete(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        var filter = GetFilter(x => x.Id, id);
        var result = await _mongoDbContext.Followers.DeleteOneAsync(filter);

        return result.DeletedCount == 1;
    }

    private FilterDefinition<ServerFollower> GetFilter<T>(Expression<Func<ServerFollower, T>> valueSelector, T value)
        => Builders<ServerFollower>.Filter.Eq(valueSelector, value);
}
