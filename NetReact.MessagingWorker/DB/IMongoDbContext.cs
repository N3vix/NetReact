using Models;
using MongoDB.Driver;

namespace NetReact.MessagingWorker.DB;

public interface IMongoDbContext
{
    IMongoCollection<ChannelMessage> Messages { get; }
}