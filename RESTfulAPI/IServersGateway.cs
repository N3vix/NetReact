using Models;

namespace RESTfulAPI;

public interface IServersGateway
{
    Task Add(params ServerDetails[] serverDetails);
    Task Edit(string id, Action<ServerDetails> editor);
    Task<ServerDetails[]> GetAll();
    Task<ServerDetails[]> GetByServerId(string[] ids);
    Task<ServerDetails> GetByServerId(string id);
}