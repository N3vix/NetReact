using System.Text;
using Microsoft.Extensions.Options;
using NetReact.MessageBroker;
using NetReact.MessageBroker.SharedModels;
using OpenTelemetry.Trace;

namespace NetReact.MessagingWorker.Services;

public class CreateChannelMessageConsumer : MessageConsumerHandlerBase<CreateChannelMessageCommand>
{
    private readonly IMessagesService _messagesService;
    private readonly IMessageBrokerProducer _createdMessageEventProducer;

    public CreateChannelMessageConsumer(
        IMessagesService messagesService,
        IOptionsSnapshot<MessageBrokerChannelConnectionConfig> options,
        IMessageBrokerProducerFactory producerFactory)
    {
        _messagesService = messagesService;

        Config = options.Get("MessageCreateCommand");

        var messageCreatedCommandConfig = options.Get("MessageCreatedCommand");
        _createdMessageEventProducer = producerFactory.Build(messageCreatedCommandConfig);
    }

    public override async Task Handle(CreateChannelMessageCommand message)
    {
        var messageId = await _messagesService.Add(message.SenderId, message.ChannelId, message.Content, message.Image);

        var messageCreatedCommand = new ChannelMessageCreated
        {
            ChannelId = message.ChannelId,
            MessageId = messageId
        };
        _createdMessageEventProducer.SendMessage(messageCreatedCommand);
    }
}

public class MessagingWorkerService : BackgroundService

{
    private readonly Tracer _tracer;
    private readonly IServiceScopeFactory _factory;
    private readonly IMessageBrokerConsumer<CreateChannelMessageCommand> _createMessageCommandConsumer;
    private readonly IMessageBrokerConsumer<EditChannelMessageCommand> _editMessageCommandConsumer;
    private readonly IMessageBrokerConsumer<DeleteChannelMessageCommand> _deleteMessageCommandConsumer;

    private readonly IMessageBrokerProducer _createdMessageEventProducer;
    private readonly IMessageBrokerProducer _editedMessageEventProducer;
    private readonly IMessageBrokerProducer _deletedMessageEventProducer;

    public MessagingWorkerService(
        Tracer tracer,
        IServiceScopeFactory factory,
        IOptionsSnapshot<MessageBrokerChannelConnectionConfig> options,
        IMessageBrokerConsumerFactory consumerFactory,
        IMessageBrokerProducerFactory producerFactory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(consumerFactory);
        ArgumentNullException.ThrowIfNull(producerFactory);

        _tracer = tracer;
        _factory = factory;

        var messageCreateCommandConfig = options.Get("MessageCreateCommand");
        _createMessageCommandConsumer = consumerFactory.Build<CreateChannelMessageCommand>(messageCreateCommandConfig);
        var messageEditCommandConfig = options.Get("MessageEditCommand");
        _editMessageCommandConsumer = consumerFactory.Build<EditChannelMessageCommand>(messageEditCommandConfig);
        var messageDeleteCommandConfig = options.Get("MessageDeleteCommand");
        _deleteMessageCommandConsumer = consumerFactory.Build<DeleteChannelMessageCommand>(messageDeleteCommandConfig);

        var messageCreatedCommandConfig = options.Get("MessageCreatedCommand");
        _createdMessageEventProducer = producerFactory.Build(messageCreatedCommandConfig);
        var messageEditedCommandConfig = options.Get("MessageEditedCommand");
        _editedMessageEventProducer = producerFactory.Build(messageEditedCommandConfig);
        var messageDeletedCommandConfig = options.Get("MessageDeletedCommand");
        _deletedMessageEventProducer = producerFactory.Build(messageDeletedCommandConfig);
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _createMessageCommandConsumer.MessageReceived += CreateMessage;
        _editMessageCommandConsumer.MessageReceived += EditMessage;
        _deleteMessageCommandConsumer.MessageReceived += DeleteMessage;

        return Task.CompletedTask;
    }

    private async void CreateMessage(CreateChannelMessageCommand command)
    {
        using var scope = _factory.CreateScope();
        var messagesService = scope.ServiceProvider.GetRequiredService<IMessagesService>();
        var messageId = await messagesService.Add(command.SenderId, command.ChannelId, command.Content, command.Image);

        var messageCreatedCommand = new ChannelMessageCreated
        {
            ChannelId = command.ChannelId,
            MessageId = messageId
        };
        _createdMessageEventProducer.SendMessage(messageCreatedCommand);
    }

    private async void EditMessage(EditChannelMessageCommand command)
    {
        using var scope = _factory.CreateScope();
        var messagesService = scope.ServiceProvider.GetRequiredService<IMessagesService>();
        var isUpdated = await messagesService.Update(command.MessageId, command.NewContent);
        if (!isUpdated) return;

        var messageEditedCommand = new ChannelMessageEdited { MessageId = command.MessageId };
        _editedMessageEventProducer.SendMessage(messageEditedCommand);
    }

    private async void DeleteMessage(DeleteChannelMessageCommand command)
    {
        using var scope = _factory.CreateScope();
        var messagesService = scope.ServiceProvider.GetRequiredService<IMessagesService>();
        var isDeleted = await messagesService.Delete(command.MessageId);
        if (!isDeleted) return;

        var messageEditedCommand = new ChannelMessageDeleted { MessageId = command.MessageId };
        _deletedMessageEventProducer.SendMessage(messageEditedCommand);
    }

    public override void Dispose()
    {
        _createMessageCommandConsumer.MessageReceived -= CreateMessage;
        _editMessageCommandConsumer.MessageReceived -= EditMessage;
        _deleteMessageCommandConsumer.MessageReceived -= DeleteMessage;

        _createMessageCommandConsumer.Dispose();
        _editMessageCommandConsumer.Dispose();
        _deleteMessageCommandConsumer.Dispose();

        _createdMessageEventProducer.Dispose();
        _editedMessageEventProducer.Dispose();
        _deletedMessageEventProducer.Dispose();
        base.Dispose();
    }
}