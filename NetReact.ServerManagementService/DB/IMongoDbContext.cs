using Models;
using MongoDB.Driver;

namespace NetReact.ServerManagementService.DB;

public interface IMongoDbContext
{
    IMongoCollection<ServerDetails> Servers { get; }
    IMongoCollection<ServerFollower> Followers { get; }
}