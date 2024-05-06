using RabbitMQ.Client.Events;

namespace NetReact.MessageBroker;

public interface IMessageBrokerConsumer
{
    void AddListener(EventHandler<BasicDeliverEventArgs> handler);
    void RemoveListener(EventHandler<BasicDeliverEventArgs> handler);
}