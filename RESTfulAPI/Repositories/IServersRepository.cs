using Models;

namespace RESTfulAPI.Repositories;

public interface IServersRepository
{
    Task<string> Add(ServerDetails serverDetails);
    Task Add(params ServerDetails[] serverDetails);
    Task<IEnumerable<ServerDetails>> Get();
    Task<ServerDetails> GetById(string id);
    Task<IEnumerable<ServerDetails>> GetByUserId(string userId);
    Task<bool> Edit(string id, ServerDetails serverDetails);
    Task<bool> Delete(string id);
}