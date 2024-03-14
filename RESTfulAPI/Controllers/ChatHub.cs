using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Models;
using RESTfulAPI.Gateways;

namespace RESTfulAPI.Controllers;

[Authorize()]
public sealed class ChatHub : Hub
{
    private IMessagesGateway MessagesGateway { get; }

    public ChatHub(IMessagesGateway messagesGateway)
    {
        MessagesGateway = messagesGateway;
    }

    public async Task JoinSpecificChat(UserConnection conn)
    {
        var userId = Context.User.Claims.First(c => c.Type == "userid").Value;

        await Groups
            .AddToGroupAsync(Context.ConnectionId, conn.ChatRoom);

        MessagesGateway.Connections[Context.ConnectionId] = conn;

        await Clients
            .Group($"{conn.ServerId}{conn.ChatRoom}")
            .SendAsync("JoinSpecificChat", userId, $"{userId} has joined");
    }

    public async Task SendMessage(string msg)
    {
        if (!MessagesGateway.Connections.TryGetValue(Context.ConnectionId, out var conn))
            return;

        var userId = Context.User.Claims.First(c => c.Type == "userid").Value;

        await Clients
            .Group($"{conn.ServerId}{conn.ChatRoom}")
            .SendAsync("ReceiveSpecificMessage", userId, msg);
    }
}
