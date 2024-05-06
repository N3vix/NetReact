namespace NetReact.MessageBroker;

public interface IMessageProducer
{
    public void SendMessage<T>(T message);
}