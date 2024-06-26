using System.Text.Json;
using NetReact.MessageBroker;
using NetReact.MessageBroker.SharedModels;

namespace NetReact.MessagingWorker.Services;

public abstract class MessageConsumerHandlerBase<T> : IMessageConsumerHandler<T>
{
    public MessageBrokerChannelConnectionConfig Config { get; protected init; }

    public abstract Task Handle(T message);

    async Task IMessageConsumerHandler.Handle(string rawMessage)
    {
        var message = GetMessage(rawMessage);
        await Handle(message);
    }

    private T GetMessage(string message) => JsonSerializer.Deserialize<T>(message)!;
}