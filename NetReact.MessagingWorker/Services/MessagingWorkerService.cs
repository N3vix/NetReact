using NetReact.MessageBroker;

namespace NetReact.MessagingWorker.Services;

public class MessagingWorkerService : BackgroundService
{
    private readonly IServiceScopeFactory _factory;
    private readonly IMessageBrokerConsumerFactory _consumerFactory;

    private readonly List<IMessageBrokerConsumer> _handlers = [];

    //TODO identical to another messagingWorkerService, refactor
    public MessagingWorkerService(
        IServiceScopeFactory factory,
        IMessageBrokerConsumerFactory consumerFactory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(consumerFactory);

        _factory = factory;
        _consumerFactory = consumerFactory;
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var scope = _factory.CreateScope();
        var consumerHandlers = scope.ServiceProvider.GetServices<IMessageConsumerHandler>();
        var consumers = consumerHandlers.Select(_consumerFactory.Build);
        _handlers.AddRange(consumers);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _handlers.ForEach(x => x.Dispose());
        base.Dispose();
    }
}