using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Models;
using RESTfulAPI.Gateways;
using RESTfulAPI.Repositories;
using SIPSorcery.Media;
using SIPSorcery.Net;
using SIPSorceryMedia.Abstractions;
using SIPSorceryMedia.Encoders;
using System.Net;
using static vpxmd.VpxCodecCxPkt.Data;

namespace RESTfulAPI.Controllers;

[Authorize]
public sealed class ChatHub : Hub
{
    private static List<SDPAudioVideoMediaFormat> AudioOfferFormats =
    [
        new(SDPMediaTypesEnum.audio, 111, "OPUS", 48000, 2, "minptime=10;useinbandfec=1")
    ];
    private static List<SDPAudioVideoMediaFormat> VideoOfferFormats =
    [
        new(SDPMediaTypesEnum.video, 98, "VP8", 90000)
    ];

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
        MessagesRepository.RtcConnections.Remove(Context.ConnectionId, out var rtcConn);
        rtcConn?.Close("connection state changed to failed");

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

    public async Task JoinVoiceChat()
    {
        if (!MessagesRepository.Connections.TryGetValue(Context.ConnectionId, out var conn))
            return;

        var userId = Context.User.Claims.First(c => c.Type == "userid").Value;

        var rtcConfig = GetConfig();
        var connection = new RTCPeerConnection(rtcConfig);

        var videoTrack = new MediaStreamTrack(SDPMediaTypesEnum.video, false, VideoOfferFormats, MediaStreamStatusEnum.SendRecv);
        connection.addTrack(videoTrack);
        var audioTrack = new MediaStreamTrack(SDPMediaTypesEnum.audio, false, AudioOfferFormats, MediaStreamStatusEnum.SendRecv);
        connection.addTrack(audioTrack);

        connection.OnRtpPacketReceived += (IPEndPoint rep, SDPMediaTypesEnum media, RTPPacket rtpPkt) =>
        {
            if (media == SDPMediaTypesEnum.audio)
            {
                foreach (var rPC in MessagesRepository.RtcConnections)
                {
                    foreach (var stream in rPC.Value.AudioStreamList)
                    {
                        rPC.Value.SendAudio(960, rtpPkt.Payload);
                    }
                }
            }
        };

        connection.OnVideoFrameReceived += (IPEndPoint remoteEP, uint timestamp, byte[] frame, VideoFormat format) =>
        {
            foreach (var rPC in MessagesRepository.RtcConnections)
            {
                foreach(var stream in rPC.Value.VideoStreamList)
                {
                    stream.SendVideo(3000, frame);
                }
            }
        };

        connection.OnTimeout += (mediaType) => Logger.LogWarning($"Timeout for {mediaType}.");
        connection.onconnectionstatechange += (state) =>
        {
            Logger.LogDebug($"Peer connection state changed to {state}.");

            if (state == RTCPeerConnectionState.closed ||
            state == RTCPeerConnectionState.disconnected ||
            state == RTCPeerConnectionState.failed)
            {
                if (!_isDisposing)
                {
                    MessagesRepository.RtcConnections.Remove(Context.ConnectionId, out var rtcConn);
                    rtcConn?.Close("connection state changed to failed");
                }
            }
            else if (state == RTCPeerConnectionState.connected)
            {
                Logger.LogDebug("Peer connection connected.");
            }
        };
        var offerSdp = connection.createOffer(null);
        await connection.setLocalDescription(offerSdp);

        MessagesRepository.RtcConnections.TryAdd(Context.ConnectionId, connection);
        await Clients
           .Group($"{conn.ServerId}/{conn.ChannelId}")
           .SendAsync("JoinVoiceChat", offerSdp, $"{userId} has joined");
    }

    private RTCConfiguration GetConfig()
    {
        RTCConfiguration rTCConfiguration = new()
        {
            iceServers =
            [
                new() { urls = "stun:stun.l.google.com:19302" },
                new() { urls = "stun:stun1.l.google.com:19302" },
                new() { urls = "stun:stun2.l.google.com:19302" },
                new() { urls = "stun:stun3.l.google.com:19302" },
                new() { urls = "stun:stun4.l.google.com:19302" }
            ]
        };
        return rTCConfiguration;
    }

    public async Task SendIce(string data)
    {
        if (!MessagesRepository.RtcConnections.TryGetValue(Context.ConnectionId, out var conn))
            return;

        var userId = Context.User.Claims.First(c => c.Type == "userid").Value;

        if (RTCIceCandidateInit.TryParse(data, out var init))
        {
            conn.addIceCandidate(init);
        }
    }

    public async Task SendAnswer(string data)
    {
        if (!MessagesRepository.RtcConnections.TryGetValue(Context.ConnectionId, out var conn))
            return;

        var userId = Context.User.Claims.First(c => c.Type == "userid").Value;

        if (RTCSessionDescriptionInit.TryParse(data, out var init))
        {
            Logger.LogDebug($"Got remote SDP, type {init.type}.");
            SetDescriptionResultEnum setDescriptionResultEnum = conn.setRemoteDescription(init);
            if (setDescriptionResultEnum != 0)
            {
                Logger.LogWarning($"Failed to set remote description, {setDescriptionResultEnum}.");
                conn?.Close("failed to set remote description");
                MessagesRepository.RtcConnections.Remove(Context.ConnectionId, out _);
            }
        }
    }

    private bool _isDisposing;
    protected override void Dispose(bool disposing)
    {
        _isDisposing = disposing;

        base.Dispose(disposing);
    }
}
