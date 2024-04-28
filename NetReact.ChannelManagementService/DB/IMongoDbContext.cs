using Models;
using MongoDB.Driver;

namespace NetReact.ChannelManagementService.DB;

public interface IMongoDbContext
{
    IMongoCollection<ChannelDetails> Channels { get; }
}