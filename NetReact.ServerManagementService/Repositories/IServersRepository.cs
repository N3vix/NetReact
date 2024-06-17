using Models;

namespace NetReact.ServerManagementService.Repositories;

public interface IServersRepository
{
    Task<string> Add(ServerDetails serverDetails);
    Task<IEnumerable<ServerDetails>> Get();
    Task<ServerDetails> GetById(string id);
    Task<IEnumerable<ServerDetails>> GetByUserId(string userId);
    Task<bool> GetIsUserFollowingServer(string userId, string serverId);
    Task Edit(string id, Action<ServerDetails> editor);
    Task Delete(string id);
    Task Save();
}