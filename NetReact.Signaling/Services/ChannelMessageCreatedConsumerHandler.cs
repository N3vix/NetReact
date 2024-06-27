using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using NetReact.MessageBroker.SharedModels;
using NetReact.MessagingWorker.Services;
using NetReact.Signaling.Controllers;
using NetReact.Signaling.Repositories;

namespace NetReact.Signaling.Services;

public class ChannelMessageCreatedConsumerHandler : MessageConsumerHandlerBase<ChannelMessageCreated>
{
    private readonly IHubContext<ChatHub> _signalHub;
    private readonly IConnectionsRepository _signalConnections;

    public ChannelMessageCreatedConsumerHandler(
        IHubContext<ChatHub> signalHub,
        IConnectionsRepository signalConnections,
        IOptionsSnapshot<MessageBrokerChannelConnectionConfig> options)
    {
        _signalHub = signalHub;
        _signalConnections = signalConnections;

        Config = options.Get("MessageCreateCommand");
    }

    public override async Task Handle(ChannelMessageCreated message)
    {
        var connection = _signalConnections.Connections.Values
            .FirstOrDefault(x => x.ChannelId.Equals(message.ChannelId));
        if (connection == null) return;

        await _signalHub
            .Clients
            .Group($"{connection.ServerId}/{connection.ChannelId}")
            .SendAsync("AddMessage", message.MessageId);
    }
}