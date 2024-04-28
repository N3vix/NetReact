using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Models;
using NetReactMonolith.Gateways;
using NetReactMonolith.Repositories;

namespace NetReactMonolith.Controllers;

[Authorize]
public sealed class ChatHub : Hub
{
    private ILogger<ChatHub> Logger { get; }
    private IConnectionsRepository ConnectionsRepository { get; }

    public ChatHub(
        ILogger<ChatHub> logger,
        IConnectionsRepository connectionsRepository)
    {
        ArgumentNullException.ThrowIfNull(connectionsRepository);
        Logger = logger;
        ConnectionsRepository = connectionsRepository;
    }

    public async Task JoinSpecificChat(UserConnection conn)
    {
        var userId = Context.User.Claims.First(c => c.Type == "userid").Value;

        await Groups
            .AddToGroupAsync(Context.ConnectionId, $"{conn.ServerId}/{conn.ChannelId}");

        ConnectionsRepository.Connections[Context.ConnectionId] = conn;

        await Clients
            .Group($"{conn.ServerId}/{conn.ChannelId}")
            .SendAsync("JoinSpecificChat", userId, $"{userId} has joined");
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        ConnectionsRepository.Connections.Remove(Context.ConnectionId, out _);

        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string messageId)
        => await UpdateMessage("AddMessage", messageId);

    public async Task DeleteMessage(string messageId)
        => await UpdateMessage("DeleteMessage", messageId);

    public async Task EditMessage(string messageId)
        => await UpdateMessage("EditMessage", messageId);

    private async Task UpdateMessage(string actionName, string messageId)
    {
        if (!ConnectionsRepository.Connections.TryGetValue(Context.ConnectionId, out var conn))
            return;

        await Clients
            .Group($"{conn.ServerId}/{conn.ChannelId}")
            .SendAsync(actionName, messageId);
    }
}
