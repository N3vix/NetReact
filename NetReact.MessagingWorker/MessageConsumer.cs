using NetReact.MessageBroker;

namespace NetReact.MessagingWorker;

public class MessageConsumer : MessageBrokerConsumerBase
{
    protected override string ExchangeKey => "testExchange";
    protected override string QueueKey => "testQueue";
    protected override string RoutingKey => "testRoute";

    public MessageConsumer(
        ILogger<MessageBrokerBase> logger, 
        MessageBrokerConnection connection)
        : base(logger, connection)
    { }
}