using Models;
using System.Collections.Concurrent;

namespace RESTfulAPI.Gateways;

public class MessagesGateway : IMessagesGateway
{
    private ConcurrentDictionary<string, UserConnection> _connections = new();

    public ConcurrentDictionary<string, UserConnection> Connections => _connections;
}
