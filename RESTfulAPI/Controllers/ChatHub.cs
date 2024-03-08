using Microsoft.AspNetCore.SignalR;

namespace RESTfulAPI.Controllers;

public sealed class ChatHub : Hub
{
    public async Task JoinChat(UserConnection connection)
    {
        await Clients.All
            .SendAsync("ReceiveMessage", "admin", $"{connection.Name} has joined");
    }

    public async Task JoinSpecificChat(UserConnection connection)
    {
        await Groups
            .AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);
        await Clients
            .Group(connection.ChatRoom)
            .SendAsync("ReceiveMessage", "admin", $"{connection.Name} has joined");
    }

    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync(
            $"{Context.User.Identity.Name} : {message}",
            CancellationToken.None);
    }
}
