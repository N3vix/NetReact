using System.Collections.Concurrent;
using Models;

namespace NetReactMonolith.Repositories;

public class MessagesRepository : IMessagesRepository
{
    private ConcurrentDictionary<string, UserConnection> _connections = new();

    public ConcurrentDictionary<string, UserConnection> Connections => _connections;


    private ConcurrentDictionary<string, WebRtcConnection> _rtcConnections = new();

    public ConcurrentDictionary<string, WebRtcConnection> RtcConnections => _rtcConnections;
}
