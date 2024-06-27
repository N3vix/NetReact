using Microsoft.Extensions.Options;
using NetReact.MessageBroker;
using NetReact.MessageBroker.SharedModels;

namespace NetReact.MessagingWorker.Services;

public class CreateChannelMessageConsumerHandler : MessageConsumerHandlerBase<CreateChannelMessageCommand>
{
    private readonly IMessagesService _messagesService;
    private readonly IMessageBrokerProducer _createdMessageEventProducer;

    public CreateChannelMessageConsumerHandler(
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