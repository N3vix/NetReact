using Models;
using System.Collections.Concurrent;

namespace RESTfulAPI.Repositories;

public class MessagesRepository : IMessagesRepository
{
    private ConcurrentDictionary<string, UserConnection> _connections = new();

    public ConcurrentDictionary<string, UserConnection> Connections => _connections;
}
