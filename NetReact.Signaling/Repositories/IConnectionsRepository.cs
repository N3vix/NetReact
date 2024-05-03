using System.Collections.Concurrent;
using Models;

namespace NetReact.Signaling.Repositories;

public interface IConnectionsRepository
{
    public ConcurrentDictionary<string, UserConnection> Connections { get; }
}
