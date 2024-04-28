using System.Collections.Concurrent;
using Models;

namespace NetReactMonolith.Repositories;

public interface IConnectionsRepository
{
    public ConcurrentDictionary<string, UserConnection> Connections { get; }
}
