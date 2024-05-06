namespace NetReact.MessageBroker;

public interface IMessageBrokerProducer
{
    public void SendMessage<T>(T message);
}