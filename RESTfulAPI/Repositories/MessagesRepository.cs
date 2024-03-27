using Models;
using SIPSorcery.Net;
using System.Collections.Concurrent;

namespace RESTfulAPI.Repositories;

public class MessagesRepository : IMessagesRepository
{
    private ConcurrentDictionary<string, UserConnection> _connections = new();

    public ConcurrentDictionary<string, UserConnection> Connections => _connections;


    private ConcurrentDictionary<string, WebRtcConnection> _rtcConnections = new();

    public ConcurrentDictionary<string, WebRtcConnection> RtcConnections => _rtcConnections;
}
