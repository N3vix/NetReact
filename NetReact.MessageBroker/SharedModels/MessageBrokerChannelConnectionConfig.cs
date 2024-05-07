namespace NetReact.MessageBroker.SharedModels;

public record MessageBrokerChannelConnectionConfig
{
    public string ExchangeKey { get; init; } = string.Empty;
    public string QueueKey { get; init; } = string.Empty;
    public string RoutingKey { get; init; } = string.Empty;
}