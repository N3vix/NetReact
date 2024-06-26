using NetReact.MessageBroker.SharedModels;

namespace NetReact.MessageBroker;

public interface IMessageConsumerHandler<in T> : IMessageConsumerHandler
{
    Task Handle(T message);
}

public interface IMessageConsumerHandler
{
    MessageBrokerChannelConnectionConfig Config { get; }

    Task Handle(string message);
}