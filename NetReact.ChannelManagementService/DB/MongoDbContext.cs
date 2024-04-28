using Models;
using MongoDB.Driver;

namespace NetReact.ChannelManagementService.DB;

public class MongoDbContext : IMongoDbContext
{
    public IMongoCollection<ChannelDetails> Channels { get; }

    public MongoDbContext(string connectionString)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(connectionString));
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase("Dd");

        Channels = database.GetCollection<ChannelDetails>("ChannelDetails");
    }
}
