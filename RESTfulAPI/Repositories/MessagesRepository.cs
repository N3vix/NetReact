using Models;
using SIPSorcery.Net;
using System.Collections.Concurrent;

namespace RESTfulAPI.Repositories;

public class MessagesRepository : IMessagesRepository
{
    private ConcurrentDictionary<string, UserConnection> _connections = new();

    public ConcurrentDictionary<string, UserConnection> Connections => _connections;


    private ConcurrentDictionary<string, RTCPeerConnection> _rtcConnections = new();

    public ConcurrentDictionary<string, RTCPeerConnection> RtcConnections => _rtcConnections;
}
