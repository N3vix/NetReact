using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Models;
using NetReact.MessagingService.Gateways;

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
        IMessagesGateway messagesGateway)
    {
        var userId = user.GetUserId();
        var image = GetByteArray(request.Image);

        var result = await messagesGateway.Add(userId, request.ChannelId, request.Content, image);
        return UnpuckResult(result);
    }

    private static async Task<IResult> GetById(
        string channelId,
        string messageId,
        ClaimsPrincipal user,
        IMessagesGateway messagesGateway)
    {
        var userId = user.GetUserId();

        var result = await messagesGateway.Get(userId, channelId, messageId);
        return UnpuckResult(result);
    }

    private static async Task<IResult> Get(
        string channelId,
        int take,
        ClaimsPrincipal user,
        IMessagesGateway messagesGateway,
        DateTime? from = null)
    {
        var userId = user.GetUserId();

        var result = await messagesGateway.Get(userId, channelId, take, from);
        return UnpuckResult(result);
    }

    private static async Task<IResult> Update(
        [FromBody] ChannelMessageUpdateRequest request,
        ClaimsPrincipal user,
        IMessagesGateway messagesGateway)
    {
        var userId = user.GetUserId();

        var result = await messagesGateway.Update(userId, request.ChannelId, request.MessageId, request.Content);
        return UnpuckResult(result);
    }

    private static async Task<IResult> Delete(
        [FromBody] ChannelMessageDeleteRequest request,
        ClaimsPrincipal user,
        IMessagesGateway messagesGateway)
    {
        var userId = user.GetUserId();

        var result = await messagesGateway.Delete(userId, request.ChannelId, request.MessageId);
        return UnpuckResult(result);
    }

    private static byte[]? GetByteArray(IFormFile? formFile)
    {
        if (formFile == null)
            return null;
        using var stream = new MemoryStream();
        formFile.CopyTo(stream);
        return stream.ToArray();
    }

    private static IResult UnpuckResult<TError>(Result<TError> result)
    {
        return result.IsSuccess
            ? Results.Ok()
            : Results.BadRequest(new { Error = result.Error });
    }

    private static IResult UnpuckResult<TValue, TError>(
        Result<TValue, TError> result,
        Func<TValue, object> valueBuilder = null)
    {
        return result.IsSuccess
            ? Results.Ok(valueBuilder == null
                ? result.Value
                : valueBuilder(result.Value))
            : Results.BadRequest(new { Error = result.Error });
    }
}