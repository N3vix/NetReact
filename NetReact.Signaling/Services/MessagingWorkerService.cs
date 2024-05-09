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
    
    private readonly IMessageBrokerConsumer _createdMessageCommandConsumer;
    private readonly IMessageBrokerConsumer _editedMessageCommandConsumer;
    private readonly IMessageBrokerConsumer _deletedMessageCommandConsumer;

    public MessagingWorkerService(
        IServiceScopeFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _factory = factory;

        using var scope = _factory.CreateScope();
        var options = scope.ServiceProvider
            .GetRequiredService<IOptionsSnapshot<MessageBrokerChannelConnectionConfig>>();
        var consumerFactory = scope.ServiceProvider.GetRequiredService<IMessageBrokerConsumerFactory>();
        var messageCreateCommandConfig = options.Get("MessageCreatedCommand");
        _createdMessageCommandConsumer = consumerFactory.Build(messageCreateCommandConfig);
        // var messageEditCommandConfig = options.Get("MessageEditedCommand");
        // _editedMessageCommandConsumer = consumerFactory.Build(messageEditCommandConfig);
        // var messageDeleteCommandConfig = options.Get("MessageDeletedCommand");
        // _deletedMessageCommandConsumer = consumerFactory.Build(messageDeleteCommandConfig);
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        _createdMessageCommandConsumer.AddListener(CreateMessage);
        // _editMessageCommandConsumer.AddListener(EditMessage);
        // _deleteMessageCommandConsumer.AddListener(DeleteMessage);

        return Task.CompletedTask;
    }

    private async void CreateMessage(object? @object, BasicDeliverEventArgs args)
    {
        using var scope = _factory.CreateScope();
        var signalHub = scope.ServiceProvider.GetRequiredService<IHubContext<ChatHub>>();
        var connectionsRepository = scope.ServiceProvider.GetRequiredService<IConnectionsRepository>();
        var command = GetCommand<ChannelMessageCreated>(args);
        
        var connection =
            connectionsRepository.Connections.Values.FirstOrDefault(x => x.ChannelId.Equals(command.ChannelId));
        if (connection == null) return;
        
        await signalHub
            .Clients
            .Group($"{connection.ServerId}/{connection.ChannelId}")
            .SendAsync("AddMessage", command.MessageId);
    }
    
    private T GetCommand<T>(BasicDeliverEventArgs args)
    {
        var body = args.Body.ToArray();
        var messageJson = Encoding.UTF8.GetString(body);
        return JsonSerializer.Deserialize<T>(messageJson)!;
    }

    public override void Dispose()
    {
        _createdMessageCommandConsumer.RemoveListener(CreateMessage);
        base.Dispose();
    }
}