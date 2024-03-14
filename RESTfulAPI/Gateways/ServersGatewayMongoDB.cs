using Models;
using MongoDB.Driver;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RESTfulAPI.Gateways;

public class ServersGatewayMongoDB : IServersGateway
{
    private readonly IMongoCollection<ServerDetails> _servers;

    public ServersGatewayMongoDB(IMongoClient mongoClient)
    {
        if (mongoClient == null) throw new ArgumentNullException(nameof(mongoClient));
        var database = mongoClient.GetDatabase("Dd");
        var collection = database.GetCollection<ServerDetails>(nameof(ServerDetails));

        _servers = collection;
    }

    public async Task<string> Add(ServerDetails serverDetails)
    {
        ArgumentNullException.ThrowIfNull(serverDetails);

        serverDetails.Id = Guid.NewGuid().ToString();

        await _servers.InsertOneAsync(serverDetails);

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
        var servers = await _servers.Find(_ => true).ToListAsync();

        return servers;
    }

    public async Task<ServerDetails> GetById(string id)
    {
        var filter = GetIdFilter(id);
        var servers = await _servers.Find(filter).FirstOrDefaultAsync();

        return servers;
    }

    public async Task<IEnumerable<ServerDetails>> GetByUserId(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Edit(string id, ServerDetails serverDetails)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        ArgumentNullException.ThrowIfNull(serverDetails);

        var filter = GetIdFilter(id);
        var update = Builders<ServerDetails>.Update.Set(x => x.Name, serverDetails.Name);

        var result = await _servers.UpdateOneAsync(filter, update);

        return result.ModifiedCount == 1;
    }

    public async Task<bool> Delete(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        var filter = GetIdFilter(id);
        var result = await _servers.DeleteOneAsync(filter);

        return result.DeletedCount == 1;
    }

    private FilterDefinition<ServerDetails> GetIdFilter(string id)
        => Builders<ServerDetails>.Filter.Eq(x => x.Id, id);
}
