using NetReact.MessageBroker;

namespace NetReact.MessagingService;

internal class MessageProducer : MessageBrokerProducerBase
{
    protected override string ExchangeKey => "testExchange";
    protected override string QueueKey => "testQueue";
    protected override string RoutingKey => "testRoute";

    public MessageProducer(
        ILogger<MessageBrokerBase> logger,
        MessageBrokerConnection connection)
        : base(logger, connection)
    { }
}