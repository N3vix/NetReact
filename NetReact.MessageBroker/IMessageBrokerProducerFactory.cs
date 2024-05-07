using NetReact.MessageBroker.SharedModels;

namespace NetReact.MessageBroker;

public interface IMessageBrokerProducerFactory
{
    IMessageBrokerProducer Build(MessageBrokerChannelConnectionConfig channelConnectionConfig);
}