using Models;

namespace NetReact.MessagingService.Services;

public interface IMessagesService
{
    Task<Result<string>> Add(string senderId, string channelId, string content, byte[]? image);
    Task<Result<ChannelMessage, string>> Get(string userId, string channelId, string messageId);

    Task<Result<IEnumerable<ChannelMessage>, string>> Get(
        string userId,
        string channelId,
        int take,
        DateTime? from = null);

    Task<Result<bool, string>> Update(
        string userId,
        string channelId,
        string messageId,
        string newContent);

    Task<Result<bool, string>> Delete(string userId, string channelId, string messageId);
}