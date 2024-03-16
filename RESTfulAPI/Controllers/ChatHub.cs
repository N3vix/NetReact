using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Models;
using RESTfulAPI.Gateways;
using RESTfulAPI.Repositories;

namespace RESTfulAPI.Controllers;

[Authorize]
public sealed class ChatHub : Hub
{
    private IMessagesRepository MessagesRepository { get; }
    private IChannelMessagesGateway MessagesGateway { get; }

    public ChatHub(IMessagesRepository messagesRepository, IChannelMessagesGateway messagesGateway)
    {
        ArgumentNullException.ThrowIfNull(messagesRepository);
        ArgumentNullException.ThrowIfNull(messagesGateway);

        MessagesRepository = messagesRepository;
        MessagesGateway = messagesGateway;
    }

    public async Task JoinSpecificChat(UserConnection conn)
    {
        var userId = Context.User.Claims.First(c => c.Type == "userid").Value;

        await Groups
            .AddToGroupAsync(Context.ConnectionId, $"{conn.ServerId}/{conn.ChannelId}");

        MessagesRepository.Connections[Context.ConnectionId] = conn;

        await Clients
            .Group($"{conn.ServerId}/{conn.ChannelId}")
            .SendAsync("JoinSpecificChat", userId, $"{userId} has joined");
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        MessagesRepository.Connections.Remove(Context.ConnectionId, out _);

        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string msg)
    {
        if (!MessagesRepository.Connections.TryGetValue(Context.ConnectionId, out var conn))
            return;

        var userId = Context.User.Claims.First(c => c.Type == "userid").Value;

        var addedMessageId = await MessagesGateway.Add(userId, conn.ChannelId, msg);
        var addedMessage = await MessagesGateway.Get(addedMessageId);

        await Clients
            .Group($"{conn.ServerId}/{conn.ChannelId}")
            .SendAsync("ReceiveSpecificMessage", addedMessage);
    }
}
