using System.Collections.Concurrent;
using Models;

namespace NetReactMonolith.Repositories;

public interface IMessagesRepository
{
    public ConcurrentDictionary<string, UserConnection> Connections { get; }
    public ConcurrentDictionary<string, WebRtcConnection> RtcConnections { get; }
}
