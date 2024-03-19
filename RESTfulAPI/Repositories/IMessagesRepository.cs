using Models;
using SIPSorcery.Net;
using System.Collections.Concurrent;

namespace RESTfulAPI.Repositories;

public interface IMessagesRepository
{
    public ConcurrentDictionary<string, UserConnection> Connections { get; }
    public ConcurrentDictionary<string, RTCPeerConnection> RtcConnections { get; }
}
