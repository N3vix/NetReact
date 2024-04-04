using Models;
using RESTfulAPI.Repositories;
using RESTfulAPI.Repositories.MongoDB;

namespace RESTfulAPI.Gateways;

public class ChannelMessagesGateway : IChannelMessagesGateway
{
    private const string UserNotFollowingError = "The user does not follow the specified server";

    private ILogger<ChannelMessagesGateway> Logger { get; }
    private IChannelMessagesRepository MessagesRepository { get; }
    private IChannelsRepository ChannelsRepository { get; }
    private IServerFollowersRepository ServerFollowersRepository { get; }

    public ChannelMessagesGateway(
        ILogger<ChannelMessagesGateway> logger,
        IChannelMessagesRepository messagesRepository,
        IChannelsRepository channelsRepository,
        IServerFollowersRepository serverFollowersRepository)
    {
        Logger = logger;
        MessagesRepository = messagesRepository;
        ChannelsRepository = channelsRepository;
        ServerFollowersRepository = serverFollowersRepository;
    }

    public async Task<Result<string>> Add(string senderId, string channelId, string content, string image)
    {
        var operationAllowed = await GetIsUserFollowingServer(senderId, channelId);
        if (!operationAllowed) return Result<string>.Failed(UserNotFollowingError);

        var channelMessage = new ChannelMessage
        {
            ChannelId = channelId,
            SenderId = senderId,
            Timestamp = DateTime.UtcNow,
            Content = content,
            Image = image
        };

        return await MessagesRepository.Add(channelMessage);
    }

    public async Task<Result<ChannelMessage>> Get(string senderId, string messageId)
    {
        var message = await MessagesRepository.GetById(messageId);

        var operationAllowed = await GetIsUserFollowingServer(senderId, message.ChannelId);
        if (!operationAllowed) return Result<ChannelMessage>.Failed(UserNotFollowingError);

        return message;
    }

    public async Task<Result<IEnumerable<ChannelMessage>>> Get(string senderId, string channelId, int take)
    {
        var operationAllowed = await GetIsUserFollowingServer(senderId, channelId);
        if (!operationAllowed) return Result<IEnumerable<ChannelMessage>>.Failed(UserNotFollowingError);

        return Result<IEnumerable<ChannelMessage>>.Successful(await MessagesRepository.Get(channelId, take, 0));
    }

    public async Task<Result<IEnumerable<ChannelMessage>>> GetBefore(string senderId, string channelId, DateTime dateTime, int take)
    {
        var operationAllowed = await GetIsUserFollowingServer(senderId, channelId);
        if (!operationAllowed) return Result<IEnumerable<ChannelMessage>>.Failed(UserNotFollowingError);

        return Result<IEnumerable<ChannelMessage>>.Successful(await MessagesRepository.GetBefore(dateTime, channelId, take));
    }

    public async Task<Result<bool>> Update(string senderId, string messageId, string newContent)
    {
        if (string.IsNullOrEmpty(newContent)) return false;

        var messageResult = await Get(senderId, messageId);
        if (!messageResult.Success) return Result<bool>.Failed(messageResult.Errors);

        var message = messageResult.Value;
        if (!senderId.Equals(message.SenderId)) return false;

        message.Content = newContent;
        message.EditedTimestamp = DateTime.UtcNow;
        return await MessagesRepository.Edit(messageId, message);
    }

    public async Task<Result<bool>> Delete(string senderId, string messageId)
    {
        var messageResult = await Get(senderId, messageId);
        if (!messageResult.Success) return Result<bool>.Failed(messageResult.Errors);

        var message = messageResult.Value;
        if (!senderId.Equals(message.SenderId)) return false;
        return await MessagesRepository.Delete(messageId);
    }

    private async Task<bool> GetIsUserFollowingServer(string senderId, string channelId)
    {
        var channelDetails = await ChannelsRepository.GetById(channelId);
        if (channelDetails == null) return false;
        return await ServerFollowersRepository.GetIsUserFollowingServer(senderId, channelDetails.ServerId);
    }
}
