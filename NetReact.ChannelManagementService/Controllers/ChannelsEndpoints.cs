using Microsoft.AspNetCore.Mvc;
using Models;
using NetReact.ChannelManagementService.Services;
using NetReact.ServiceSetup;

namespace NetReact.ChannelManagementService.Controllers;

internal static class ChannelsEndpoints
{
    public static void SetupChannelsEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("channels").RequireAuthorization();

        group.MapPost("", CreateChannel);
        group.MapGet("{serverId}/all", GetChannels);
        group.MapGet("{id}", GetChannel);
        group.MapGet("{channelId}/user", GetIsFollowing);
    }

    private static async Task<IResult> CreateChannel(
        [FromBody] ChannelAddRequest request,
        IChannelsService channelsService)
    {
        var result = await channelsService.CreateServer(request.ServerId, request.Name, request.Type);
        return result.UnpuckResult();
    }

    private static async Task<IResult> GetChannels(
        string serverId,
        IChannelsService channelsService)
    {
        var result = await channelsService.GetChannels(serverId);
        return result.UnpuckResult();
    }

    private static async Task<IResult> GetChannel(
        string id,
        IChannelsService channelsService)
    {
        var result = await channelsService.GetChannel(id);
        return result.UnpuckResult();
    }
    
    private static async Task<IResult> GetIsFollowing(
        string channelId,
        IChannelsService channelsService)
    {
        var result = await channelsService.GetIsFollowing(channelId);
        return result.UnpuckResult();
    }
}