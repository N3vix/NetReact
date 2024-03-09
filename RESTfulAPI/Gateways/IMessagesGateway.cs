using Models;
using System.Collections.Concurrent;

namespace RESTfulAPI.Gateways;

public interface IMessagesGateway
{
    public ConcurrentDictionary<string, UserConnection> Connections { get; }
}
