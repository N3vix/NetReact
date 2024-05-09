using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Models;
using NetReact.MessageBroker;
using NetReact.MessageBroker.SharedModels;
using NetReact.Signaling.Repositories;
using RabbitMQ.Client.Events;

namespace NetReact.Signaling.Controllers;

public interface IChatHub
{
    
}

[Authorize]
public sealed class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _logger;
    private readonly IConnectionsRepository _connectionsRepository;

    private readonly IMessageBrokerConsumer _createdMessageCommandConsumer;
    private readonly IMessageBrokerConsumer _editedMessageCommandConsumer;
    private readonly IMessageBrokerConsumer _deletedMessageCommandConsumer;

    public ChatHub(
        ILogger<ChatHub> logger,
        IConnectionsRepository connectionsRepository,
        IOptionsSnapshot<MessageBrokerChannelConnectionConfig> options,
        IMessageBrokerConsumerFactory consumerFactory)
    {
        ArgumentNullException.ThrowIfNull(connectionsRepository);
        _logger = logger;
        _connectionsRepository = connectionsRepository;
    }

    public async Task JoinSpecificChat(UserConnection conn)
    {
        var userId = Context.User.Claims.First(c => c.Type == "userid").Value;

        await Groups
            .AddToGroupAsync(Context.ConnectionId, $"{conn.ServerId}/{conn.ChannelId}");

        _connectionsRepository.Connections[Context.ConnectionId] = conn;

        await Clients
            .Group($"{conn.ServerId}/{conn.ChannelId}")
            .SendAsync("JoinSpecificChat", userId, $"{userId} has joined");
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _connectionsRepository.Connections.Remove(Context.ConnectionId, out _);

        return base.OnDisconnectedAsync(exception);
    }

    // private async void OnMessageCreated(object? @object, BasicDeliverEventArgs args)
    // {
    //     var command = GetCommand<ChannelMessageCreated>(args);
    //     // await UpdateMessage("AddMessage", GetCommand<ChannelMessageCreated>(args).MessageId);
    //     // if (!_connectionsRepository.Connections.TryGetValue(Context.ConnectionId, out var conn))
    //     //     return;
    //
    //     var connection =
    //         _connectionsRepository.Connections.Values.FirstOrDefault(x => x.ChannelId.Equals(command.ChannelId));
    //     if (connection == null) return;
    //
    //     await Clients
    //         .Group($"{connection.ServerId}/{connection.ChannelId}")
    //         .SendAsync("AddMessage", command.MessageId);
    // }

    public async Task DeleteMessage(string messageId)
        => await UpdateMessage("DeleteMessage", messageId);

    public async Task EditMessage(string messageId)
        => await UpdateMessage("EditMessage", messageId);

    private async Task UpdateMessage(string actionName, string messageId)
    {
        if (!_connectionsRepository.Connections.TryGetValue(Context.ConnectionId, out var conn))
            return;

        await Clients
            .Group($"{conn.ServerId}/{conn.ChannelId}")
            .SendAsync(actionName, messageId);
    }
}