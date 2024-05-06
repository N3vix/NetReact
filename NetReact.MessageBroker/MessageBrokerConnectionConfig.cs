namespace NetReact.MessageBroker;

public class MessageBrokerConnectionConfig
{
    public string HostName { get; init; } = string.Empty;
    public int Port { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}