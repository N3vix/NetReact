using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Models;
using NetReact.ServerManagementService.Services;

namespace NetReact.ServerManagementService.Controllers;

public static class ServersEndpoints
{
    public static void MapServersEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("servers").RequireAuthorization();

        group.MapPost("", CreateServer);
        group.MapGet("", GetAllServers);
        group.MapGet("user", GetFollowingServers);
        group.MapGet("{id}", GetServer);
        group.MapGet("{serverId}/user", GetIsFollowing);
    }

    private static async Task<string> CreateServer(
        [FromBody] ServerAddRequest request,
        IServersService serversService)
    {
        return await serversService.CreateServer(request.Name);
    }

    private static async Task<IEnumerable<ServerDetails>> GetAllServers(IServersService serversService)
    {
        return await serversService.GetAllServers();
    }

    private static async Task<IEnumerable<ServerDetails>> GetFollowingServers(
        ClaimsPrincipal user,
        IServersService serversService)
    {
        return await serversService.GetFollowedServers(user.GetUserId());
    }

    private static async Task<ServerDetails> GetServer(string id, IServersService serversService)
    {
        return await serversService.GetServer(id);
    }

    private static async Task<bool> GetIsFollowing(
        string serverId,
        ClaimsPrincipal user,
        IServersService serversService)
    {
        return await serversService.GetIsFollowing(user.GetUserId(), serverId);
    }
}