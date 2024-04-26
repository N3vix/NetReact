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
    private IMessagesRepository MessagesRepository { get; }
    private IChannelMessagesGateway MessagesGateway { get; }

    public ChatHub(
        ILogger<ChatHub> logger,
        IMessagesRepository messagesRepository,
        IChannelMessagesGateway messagesGateway)
    {
        ArgumentNullException.ThrowIfNull(messagesRepository);
        ArgumentNullException.ThrowIfNull(messagesGateway);
        Logger = logger;
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
        //foreach (var rPC in MessagesRepository.RtcConnections)
        //{
        //    rPC.Value.RemoveTrack(Context.ConnectionId);
        //}
        //MessagesRepository.RtcConnections.Remove(Context.ConnectionId, out var rtcConn);
        //rtcConn?.Dispose();

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
        if (!MessagesRepository.Connections.TryGetValue(Context.ConnectionId, out var conn))
            return;

        await Clients
            .Group($"{conn.ServerId}/{conn.ChannelId}")
            .SendAsync(actionName, messageId);
    }

    //public async Task JoinVoiceChat()
    //{
    //    if (!MessagesRepository.Connections.TryGetValue(Context.ConnectionId, out var conn))
    //        return;

    //    var newConnection = new WebRtcConnection(Context.ConnectionId);
    //    newConnection.AddTrack(Context.ConnectionId);
    //    foreach (var rPC in MessagesRepository.RtcConnections)
    //    {
    //        newConnection.AddTrack(rPC.Key);
    //        //rPC.Value.AddTrack(Context.ConnectionId);
    //    }
    //    MessagesRepository.RtcConnections.TryAdd(Context.ConnectionId, newConnection);

    //    newConnection.OnVideoFrameReceived += (string connectionId, byte[] frame) =>
    //    {
    //        foreach (var rPC in MessagesRepository.RtcConnections)
    //        {
    //            rPC.Value.SendFrame(connectionId, frame);
    //        }
    //    };

    //    //connection.OnTimeout += (mediaType) => Logger.LogWarning($"Timeout for {mediaType}.");
    //    newConnection.OnConnectionStateChanged += (state) =>
    //    {
    //        Logger.LogDebug($"Peer connection state changed to {state}.");

    //        if (state == RTCPeerConnectionState.closed ||
    //        state == RTCPeerConnectionState.disconnected ||
    //        state == RTCPeerConnectionState.failed)
    //        {
    //            if (!_isDisposing)
    //            {
    //                MessagesRepository.RtcConnections.Remove(Context.ConnectionId, out var rtcConn);
    //                rtcConn?.Dispose();
    //            }
    //        }
    //        else if (state == RTCPeerConnectionState.connected)
    //        {
    //            Logger.LogDebug("Peer connection connected.");
    //        }
    //    };

    //    await CreateOffer(conn, newConnection);
    //}

    //private async Task CreateOffer(UserConnection conn, WebRtcConnection newConnection)
    //{
    //    var userId = Context.User.Claims.First(c => c.Type == "userid").Value;

    //    var offerSdp = await newConnection.CreateOffer();
    //    await Clients
    //       .Group($"{conn.ServerId}/{conn.ChannelId}")
    //       .SendAsync("JoinVoiceChat", offerSdp, $"{userId} has joined");
    //}

    //public async Task SendIce(string data)
    //{
    //    if (!MessagesRepository.RtcConnections.TryGetValue(Context.ConnectionId, out var conn))
    //        return;

    //    var userId = Context.User.Claims.First(c => c.Type == "userid").Value;

    //    conn.AddIceCandidate(data);
    //}

    //public async Task SendAnswer(string data)
    //{
    //    if (!MessagesRepository.RtcConnections.TryGetValue(Context.ConnectionId, out var conn))
    //        return;

    //    var userId = Context.User.Claims.First(c => c.Type == "userid").Value;

    //    var setDescriptionResultEnum = conn.AddRemoteDescription(data);

    //    if (setDescriptionResultEnum != 0)
    //    {
    //        Logger.LogWarning($"Failed to set remote description, {setDescriptionResultEnum}.");
    //        conn?.Dispose();
    //        MessagesRepository.RtcConnections.Remove(Context.ConnectionId, out _);
    //    }
    //}

    //private bool _isDisposing;
    //protected override void Dispose(bool disposing)
    //{
    //    _isDisposing = disposing;

    //    base.Dispose(disposing);
    //}
}
