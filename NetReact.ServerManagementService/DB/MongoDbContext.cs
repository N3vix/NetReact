using Models;
using MongoDB.Driver;

namespace NetReact.ServerManagementService.DB;

public class MongoDbContext : IMongoDbContext
{
    public IMongoCollection<ServerDetails> Servers { get; }

    public IMongoCollection<ServerFollower> Followers { get; }

    public MongoDbContext(string connectionString)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(connectionString));
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase("Dd");

        Servers = database.GetCollection<ServerDetails>("ServerDetails");
        Followers = database.GetCollection<ServerFollower>("ServerFollowers");
    }
}