namespace NetReact.MessageBroker;

public interface IMessageBrokerProducer : IDisposable
{
    public void SendMessage<T>(T message);
}