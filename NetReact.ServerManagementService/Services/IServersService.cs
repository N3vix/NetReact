using Microsoft.AspNetCore.Mvc;
using Models;

namespace NetReact.ServerManagementService.Services;

public interface IServersService
{
    Task<string> CreateServer(string name);
    Task<IEnumerable<ServerDetails>> GetAllServers();
    Task<IEnumerable<ServerDetails>> GetFollowedServers(string userId);
    Task<ServerDetails> GetServer([FromQuery] string id);
    Task<bool> GetIsFollowing(string userId, string serverId);
}