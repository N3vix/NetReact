using Microsoft.Extensions.Options;
using NetReact.MessageBroker;
using NetReact.MessageBroker.SharedModels;

namespace NetReact.MessagingWorker.Services;

public class EditChannelMessageConsumerHandler : MessageConsumerHandlerBase<EditChannelMessageCommand>
{
    private readonly IMessagesService _messagesService;
    private readonly IMessageBrokerProducer _editedMessageEventProducer;

    public EditChannelMessageConsumerHandler(
        IMessagesService messagesService,
        IOptionsSnapshot<MessageBrokerChannelConnectionConfig> options,
        IMessageBrokerProducerFactory producerFactory)
    {
        _messagesService = messagesService;

        Config = options.Get("MessageEditCommand");

        var messageEditedCommandConfig = options.Get("MessageEditedCommand");
        _editedMessageEventProducer = producerFactory.Build(messageEditedCommandConfig);
    }

    public override async Task Handle(EditChannelMessageCommand message)
    {
        var isUpdated = await _messagesService.Update(message.MessageId, message.NewContent);

        if (!isUpdated) return;

        var messageEditedCommand = new ChannelMessageEdited { MessageId = message.MessageId };
        _editedMessageEventProducer.SendMessage(messageEditedCommand);
    }
}