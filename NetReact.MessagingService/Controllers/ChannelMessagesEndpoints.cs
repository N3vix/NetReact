using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Models;
using NetReact.MessagingService.Services;
using NetReact.ServiceSetup;

namespace NetReact.MessagingService.Controllers;

public static class ChannelMessagesEndpoints
{
    public static void MapChannelMessagesEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("channel").RequireAuthorization();

        group.MapPost("messages", Add).DisableAntiforgery();
        group.MapGet("{channelId}/messages/{messageId}", GetById);
        group.MapGet("{channelId}/messages", Get);
        group.MapPut("messages", Update);
        group.MapDelete("messages", Delete);
    }

    private static async Task<IResult> Add(
        [FromForm] ChannelMessageAddRequest request,
        ClaimsPrincipal user,
        IMessagesService messagesService)
    {
        var userId = user.GetUserId();
        var image = GetByteArray(request.Image);

        var result = await messagesService.Add(userId, request.ChannelId, request.Content, image);
        return result.UnpuckResult();
    }

    private static async Task<IResult> GetById(
        string channelId,
        string messageId,
        ClaimsPrincipal user,
        IMessagesService messagesService)
    {
        var userId = user.GetUserId();

        var result = await messagesService.Get(userId, channelId, messageId);
        return result.UnpuckResult();
    }

    private static async Task<IResult> Get(
        string channelId,
        int take,
        ClaimsPrincipal user,
        IMessagesService messagesService,
        DateTime? from = null)
    {
        var userId = user.GetUserId();

        var result = await messagesService.Get(userId, channelId, take, from);
        return result.UnpuckResult();
    }

    private static async Task<IResult> Update(
        [FromBody] ChannelMessageUpdateRequest request,
        ClaimsPrincipal user,
        IMessagesService messagesService)
    {
        var userId = user.GetUserId();

        var result = await messagesService.Update(userId, request.ChannelId, request.MessageId, request.Content);
        return result.UnpuckResult();
    }

    private static async Task<IResult> Delete(
        [FromBody] ChannelMessageDeleteRequest request,
        ClaimsPrincipal user,
        IMessagesService messagesService)
    {
        var userId = user.GetUserId();

        var result = await messagesService.Delete(userId, request.ChannelId, request.MessageId);
        return result.UnpuckResult();
    }

    private static byte[]? GetByteArray(IFormFile? formFile)
    {
        if (formFile == null)
            return null;
        using var stream = new MemoryStream();
        formFile.CopyTo(stream);
        return stream.ToArray();
    }
}