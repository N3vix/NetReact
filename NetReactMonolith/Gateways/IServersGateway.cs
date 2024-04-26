using Microsoft.AspNetCore.Mvc;
using Models;

namespace NetReactMonolith.Gateways;

public interface IServersGateway
{
    Task<string> CreateServer(string name);
    Task<IEnumerable<ServerDetails>> GetAllServers();
    Task<IEnumerable<ServerDetails>> GetFollowedServers(string userId);
    Task<ServerDetails> GetServer([FromQuery] string id);
}