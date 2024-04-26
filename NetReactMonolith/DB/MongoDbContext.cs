using Models;
using MongoDB.Driver;

namespace NetReactMonolith.DB;

public class MongoDbContext : IMongoDbContext
{
    public IMongoCollection<ServerDetails> Servers { get; }

    public IMongoCollection<ServerFollower> Followers { get; }

    public IMongoCollection<ChannelDetails> Channels { get; }

    public IMongoCollection<ChannelMessage> ChannelMessages { get; }

    public MongoDbContext(string connectionString)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(connectionString));
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase("Dd");

        Servers = database.GetCollection<ServerDetails>("ServerDetails");
        Followers = database.GetCollection<ServerFollower>("ServerFollowers");
        Channels = database.GetCollection<ChannelDetails>("ChannelDetails");
        ChannelMessages = database.GetCollection<ChannelMessage>("ChannelMessages");
    }
}
