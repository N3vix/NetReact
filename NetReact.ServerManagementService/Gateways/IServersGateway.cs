﻿using Microsoft.AspNetCore.Mvc;
using Models;

namespace NetReact.ServerManagementService.Gateways;

public interface IServersGateway
{
    Task<string> CreateServer(string name);
    Task<IEnumerable<ServerDetails>> GetAllServers();
    Task<IEnumerable<ServerDetails>> GetFollowedServers(string userId);
    Task<ServerDetails> GetServer([FromQuery] string id);
    Task<bool> GetIsFollowing(string userId, string serverId);
}