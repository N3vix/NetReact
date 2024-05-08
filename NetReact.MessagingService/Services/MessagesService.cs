using Microsoft.Extensions.Options;
using Models;
using NetReact.MessageBroker;
using NetReact.MessageBroker.SharedModels;
using NetReact.MessagingService.Repositories;

namespace NetReact.MessagingService.Services;

public class MessagesService : IMessagesService
{
    private readonly ILogger<MessagesService> _logger;
    private readonly IMessagesRepository _messagesRepository;
    private readonly IMessageMediaService _messageMediaService;
    private readonly MessagesServiceHttpClient _httpClient;
    private readonly IMessageBrokerProducer _messageProducer;

    public MessagesService(
        ILogger<MessagesService> logger,
        IOptionsSnapshot<MessageBrokerChannelConnectionConfig> options,
        IMessagesRepository messagesRepository,
        IMessageMediaService messageMediaService,
        MessagesServiceHttpClient httpClient,
        IMessageBrokerProducerFactory messageProducer)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(messagesRepository);
        ArgumentNullException.ThrowIfNull(messageMediaService);
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(messageProducer);

        _logger = logger;
        _messagesRepository = messagesRepository;
        _messageMediaService = messageMediaService;
        _httpClient = httpClient;
        var channelConnectionConfig = options.Get("MessageCreated");
        _messageProducer = messageProducer.Build(channelConnectionConfig);
    }

    public async Task<Result<string>> Add(string senderId, string channelId, string content, byte[]? image)
    {
        var isFollowing = await IsFollowing(senderId, channelId);
        if (isFollowing.IsError)
            return isFollowing.Error;

        var imageName = await _messageMediaService.WriteAsync(image);

        var messageCreated = new CreateChannelMessageCommand
        {
            SenderId = senderId,
            ChannelId = channelId,
            Content = content,
            Image = imageName
        };
        _messageProducer.SendMessage(messageCreated);

        return Result<string>.Successful();
    }

    public async Task<Result<ChannelMessage, string>> Get(string userId, string channelId, string messageId)
    {
        var isFollowing = await IsFollowing(userId, channelId);
        if (isFollowing.IsError)
            return isFollowing.Error;

        return await _messagesRepository.GetById(messageId);
    }

    public async Task<Result<IEnumerable<ChannelMessage>, string>> Get(
        string userId,
        string channelId,
        int take,
        DateTime? from = null)
    {
        var isFollowing = await IsFollowing(userId, channelId);
        if (isFollowing.IsError)
            return isFollowing.Error;

        var messages = await _messagesRepository.Get(channelId, take, from);
        return Result<IEnumerable<ChannelMessage>, string>.Successful(messages);
    }

    public async Task<Result<bool, string>> Update(
        string userId,
        string channelId,
        string messageId,
        string newContent)
    {
        var messageResult = await Get(userId, channelId, messageId);
        if (messageResult.IsError) return messageResult.Error;

        var message = messageResult.Value;
        if (!message.SenderId.Equals(userId)) return "Does not belong to the user";
        if (string.IsNullOrEmpty(newContent)) return "Content cannot be empty.";

        message.Content = newContent;
        message.EditedTimestamp = DateTime.UtcNow;
        return await _messagesRepository.Edit(messageId, message);
    }

    public async Task<Result<bool, string>> Delete(string userId, string channelId, string messageId)
    {
        var messageResult = await Get(userId, channelId, messageId);
        if (messageResult.IsError) return messageResult.Error;

        var message = messageResult.Value;
        if (!message.SenderId.Equals(userId)) return "Does not belong to the user";
        
        return await _messagesRepository.Delete(messageId);
    }

    private async Task<Result<bool, string>> IsFollowing(string userId, string channelId)
    {
        var response = await _httpClient.GetIsFollowingServer(userId, channelId);
        if (!response.IsSuccessStatusCode)
            return "Channel management service failed.";

        var content = await response.Content.ReadAsStringAsync();
        if (!bool.TryParse(content, out var result) || !result)
            return "Operation not allowed.";

        return true;
    }
}