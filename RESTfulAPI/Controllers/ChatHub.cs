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
        await Groups
            .AddToGroupAsync(Context.ConnectionId, conn.ChatRoom);

        MessagesGateway.Connections[Context.ConnectionId] = conn;

        await Clients
            .Group(conn.ChatRoom)
            .SendAsync("JoinSpecificChat", conn.Username, $"{conn.Username} has joined");
    }

    public async Task SendMessage(string msg)
    {
        if (!MessagesGateway.Connections.TryGetValue(Context.ConnectionId, out var conn))
            return;
        await Clients
            .Group(conn.ChatRoom)
            .SendAsync("ReceiveSpecificMessage", conn.Username, msg);
    }
}
