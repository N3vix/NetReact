using Microsoft.Extensions.Options;
using NetReact.MessageBroker;
using NetReact.MessageBroker.SharedModels;

namespace NetReact.MessagingWorker.Services;

public class DeleteMessageConsumerHandler : MessageConsumerHandlerBase<DeleteChannelMessageCommand>
{
    private readonly IMessagesService _messagesService;
    private readonly IMessageBrokerProducer _deletedMessageEventProducer;

    public DeleteMessageConsumerHandler(
        IMessagesService messagesService,
        IOptionsSnapshot<MessageBrokerChannelConnectionConfig> options,
        IMessageBrokerProducerFactory producerFactory)
    {
        _messagesService = messagesService;

        Config = options.Get("MessageDeleteCommand");

        var messageDeletedCommandConfig = options.Get("MessageDeletedCommand");
        _deletedMessageEventProducer = producerFactory.Build(messageDeletedCommandConfig);
    }

    public override async Task Handle(DeleteChannelMessageCommand message)
    {
        var isDeleted = await _messagesService.Delete(message.MessageId);
        if (!isDeleted) return;

        var messageDeletedCommand = new ChannelMessageDeleted { MessageId = message.MessageId };
        _deletedMessageEventProducer.SendMessage(messageDeletedCommand);
    }
}