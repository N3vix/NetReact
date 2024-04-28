using Models;
using MongoDB.Driver;
using NetReact.ServerManagementService.DB;

namespace NetReact.ServerManagementService.Repositories;

public class ServersRepository : IServersRepository
{
    private readonly IMongoDbContext _mongoDbContext;

    public ServersRepository(IMongoDbContext mongoDbContext)
    {
        ArgumentNullException.ThrowIfNull(mongoDbContext);

        _mongoDbContext = mongoDbContext;
    }

    public async Task<string> Add(ServerDetails serverDetails)
    {
        ArgumentNullException.ThrowIfNull(serverDetails);

        serverDetails.Id = Guid.NewGuid().ToString();

        await _mongoDbContext.Servers.InsertOneAsync(serverDetails);

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
        var servers = await _mongoDbContext.Servers.Find(_ => true).ToListAsync();

        return servers;
    }

    public async Task<ServerDetails> GetById(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        var filter = GetIdFilter(id);
        var servers = await _mongoDbContext.Servers.Find(filter).FirstOrDefaultAsync();

        return servers;
    }

    public async Task<IEnumerable<ServerDetails>> GetByUserId(string userId)
    {
        ArgumentException.ThrowIfNullOrEmpty(userId);

        var serverFollowerFilter = Builders<ServerFollower>.Filter.Eq(x => x.UserId, userId);
        var userServers = await _mongoDbContext.Followers.Find(serverFollowerFilter).ToListAsync();

        var serverDetailsFilter = Builders<ServerDetails>.Filter.In(x => x.Id, userServers.Select(x => x.ServerId));
        var servers = await _mongoDbContext.Servers.Find(serverDetailsFilter).ToListAsync();

        return servers;
    }

    public async Task<bool> Edit(string id, ServerDetails serverDetails)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        ArgumentNullException.ThrowIfNull(serverDetails);

        var filter = GetIdFilter(id);
        var update = Builders<ServerDetails>.Update.Set(x => x.Name, serverDetails.Name);

        var result = await _mongoDbContext.Servers.UpdateOneAsync(filter, update);

        return result.ModifiedCount == 1;
    }

    public async Task<bool> Delete(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        var filter = GetIdFilter(id);
        var result = await _mongoDbContext.Servers.DeleteOneAsync(filter);

        return result.DeletedCount == 1;
    }

    private FilterDefinition<ServerDetails> GetIdFilter(string id)
        => Builders<ServerDetails>.Filter.Eq(x => x.Id, id);
}
