using Models;
using MongoDB.Driver;

namespace NetReact.MessagingService.DB;

public class MongoDbContext : IMongoDbContext
{
    public IMongoCollection<ChannelMessage> Messages { get; }

    public MongoDbContext(string connectionString)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(connectionString));
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase("Dd");

        Messages = database.GetCollection<ChannelMessage>("Messages");
    }
}