using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using NetReact.MessageBroker;
using NetReact.MessageBroker.SharedModels;
using NetReact.Signaling.Controllers;
using NetReact.Signaling.Repositories;
using RabbitMQ.Client.Events;

namespace NetReact.Signaling.Services;

internal class MessagingWorkerService : BackgroundService
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
        
        //todo
        // var messageEditCommandConfig = options.Get("MessageEditedCommand");
        // _editedMessageCommandConsumer = consumerFactory.Build(messageEditCommandConfig);
        // var messageDeleteCommandConfig = options.Get("MessageDeletedCommand");
        // _deletedMessageCommandConsumer = consumerFactory.Build(messageDeleteCommandConfig);
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