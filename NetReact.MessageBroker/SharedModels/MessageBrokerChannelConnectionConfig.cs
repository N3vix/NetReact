namespace NetReact.MessageBroker.SharedModels;

public record MessageBrokerChannelConnectionConfig
{
    public string Exchange { get; init; } = string.Empty;
    public string Queue { get; init; } = string.Empty;
    public string Routing { get; init; } = string.Empty;
}