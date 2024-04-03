using Microsoft.AspNetCore.Mvc;
using Models;

namespace RESTfulAPI.Gateways;

public interface IServersGateway
{
    Task<IEnumerable<ServerDetails>> GetAllServers();
    Task<IEnumerable<ServerDetails>> GetFollowedServers(string userId);
    Task<ServerDetails> GetServer([FromQuery] string id);
}