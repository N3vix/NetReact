using Models;
using MongoDB.Driver;

namespace RESTfulAPI.DB;

public class MongoDbContext : IMongoDbContext
{
    public IMongoCollection<ServerDetails> Servers { get; }

    public IMongoCollection<ServerFollower> Followers { get; }

    public MongoDbContext(string connectionString)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(connectionString));
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase("Dd");
        var servers = database.GetCollection<ServerDetails>(nameof(ServerDetails));
        var followers = database.GetCollection<ServerFollower>("ServerFollowers");

        Servers = servers;
        Followers = followers;
    }
}
