using NetReact.MessageBroker.SharedModels;

namespace NetReact.MessageBroker;

public interface IMessageBrokerConsumerFactory
{
    IMessageBrokerConsumer Build(IMessageConsumerHandler consumerHandler);
}