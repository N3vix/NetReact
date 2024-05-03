using System.Collections.Concurrent;
using Models;

namespace NetReact.Signaling.Repositories;

public class ConnectionsRepository : IConnectionsRepository
{
    private ConcurrentDictionary<string, UserConnection> _connections = new();

    public ConcurrentDictionary<string, UserConnection> Connections => _connections;
}
