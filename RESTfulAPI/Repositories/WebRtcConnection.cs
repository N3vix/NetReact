using SIPSorcery.Net;
using SIPSorceryMedia.Abstractions;
using System.Collections.Concurrent;
using System.Net;

namespace RESTfulAPI.Repositories;

public class WebRtcConnection : IDisposable
{
    private static List<SDPAudioVideoMediaFormat> AudioOfferFormats =
    [
        new(SDPMediaTypesEnum.audio, 111, "OPUS", 48000, 2, "minptime=10;useinbandfec=1")
    ];
    private static List<SDPAudioVideoMediaFormat> VideoOfferFormats =
    [
        new(SDPMediaTypesEnum.video, 98, "VP8", 90000)
    ];

    private readonly string _connectionId;

    private readonly RTCPeerConnection _rtcPeerConnection;

    private readonly Dictionary<string, MediaStreamTrack> _mediaTracks = new();

    public event Action<string, byte[]> OnVideoFrameReceived;

    private void InvokeOnVideoFrameReceived(string id, byte[] frame)
        => OnVideoFrameReceived?.Invoke(id, frame);

    public event Action<RTCPeerConnectionState> OnConnectionStateChanged;

    private void InvokeOnConnectionStateChanged(RTCPeerConnectionState newState)
        => OnConnectionStateChanged?.Invoke(newState);

    public event Action OnNegotiationNeeded;

    private void InvokeOnNegotiationNeeded()
        => OnNegotiationNeeded?.Invoke();

    public WebRtcConnection(string connectionId)
    {
        _connectionId = connectionId;
        _rtcPeerConnection = new RTCPeerConnection(GetConfig());

        InitDefaultTracks();

        _rtcPeerConnection.OnVideoFrameReceived += OnConnVideoFrameReceived;
        _rtcPeerConnection.onconnectionstatechange += OnConnStateChanged;
        _rtcPeerConnection.onnegotiationneeded += OnConnNegotiationNeeded;
    }

    private void OnConnVideoFrameReceived(IPEndPoint remoteEP, uint timestamp, byte[] frame, VideoFormat format)
        => InvokeOnVideoFrameReceived(_connectionId, frame);

    private void OnConnStateChanged(RTCPeerConnectionState newState)
        => InvokeOnConnectionStateChanged(newState);

    private void OnConnNegotiationNeeded() 
        => InvokeOnNegotiationNeeded();

    private void InitDefaultTracks()
    {
        AddTrack("", MediaStreamStatusEnum.RecvOnly);

        //var audioTrack = new MediaStreamTrack(SDPMediaTypesEnum.audio, false, AudioOfferFormats, MediaStreamStatusEnum.SendRecv);
        //connection.addTrack(audioTrack);

        //connection.OnRtpPacketReceived += async (IPEndPoint rep, SDPMediaTypesEnum media, RTPPacket rtpPkt) =>
        //{
        //    if (media == SDPMediaTypesEnum.audio)
        //    {
        //        foreach (var rPC in MessagesRepository.RtcConnections)
        //        {
        //            foreach (var stream in rPC.Value.AudioStreamList)
        //            {
        //                rPC.Value.SendAudio(960, rtpPkt.Payload);
        //            }
        //        }
        //    }
        //};
    }

    public async Task<RTCSessionDescriptionInit> CreateOffer()
    {
        var offer = _rtcPeerConnection.createOffer(null);
        await _rtcPeerConnection.setLocalDescription(offer);
        return offer;
    }
    
    public void AddIceCandidate(string iceData)
    {
        if (RTCIceCandidateInit.TryParse(iceData, out var init))
        {
            _rtcPeerConnection.addIceCandidate(init);
        }
    }

    public SetDescriptionResultEnum AddRemoteDescription(string remoteDescriptionData)
    {
        if (RTCSessionDescriptionInit.TryParse(remoteDescriptionData, out var init))
        {
            return _rtcPeerConnection.setRemoteDescription(init);
        }
        return SetDescriptionResultEnum.Error;
    }

    public void SendFrame(string connectionId, byte[] frame)
    {
        var index = _mediaTracks.IndexOfKey(connectionId);
        if (index == -1) return;

        _rtcPeerConnection.VideoStreamList[index].SendVideo(3000, frame);
    }

    public void AddTrack(string connectionId)
        => AddTrack(connectionId, MediaStreamStatusEnum.SendOnly);

    private void AddTrack(string connectionId, MediaStreamStatusEnum status)
    {
        var track = CreateTrack(status);

        _mediaTracks[connectionId] = track;
        _rtcPeerConnection.addTrack(track);
    }

    private MediaStreamTrack CreateTrack(MediaStreamStatusEnum status)
        => new MediaStreamTrack(SDPMediaTypesEnum.video, false, VideoOfferFormats, status);

    public void RemoveTrack(string connectionId)
    {
        _mediaTracks.Remove(connectionId, out var track);
        _rtcPeerConnection.removeTrack(track);
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

    public void Dispose()
    {
        _rtcPeerConnection.OnVideoFrameReceived -= OnConnVideoFrameReceived;
        _rtcPeerConnection.onconnectionstatechange -= OnConnStateChanged;
        _rtcPeerConnection.onnegotiationneeded -= OnConnNegotiationNeeded;
        _rtcPeerConnection.Dispose();
    }
}

public static class DictionaryExtensions
{
    public static int IndexOfKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
        return Array.IndexOf(dictionary.Keys.ToArray(), key);
    }
}