using Models;
using MongoDB.Driver;

namespace NetReact.MessagingService.DB;

public interface IMongoDbContext
{
    IMongoCollection<ChannelMessage> Messages { get; }
}