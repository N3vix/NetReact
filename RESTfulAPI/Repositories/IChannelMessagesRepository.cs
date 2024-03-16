using Models;

namespace RESTfulAPI.Repositories;

public interface IChannelMessagesRepository
{
    Task<string> Add(ChannelMessage channelDetails);
    Task<IEnumerable<ChannelMessage>> Get(string channelId, int take, int skip);
    Task<IEnumerable<ChannelMessage>> GetBefore(DateTime dateTime, string channelId, int take);
    Task<ChannelMessage> GetById(string id);
    Task<bool> Edit(string id, ChannelMessage message);
    Task<bool> Delete(string id);
}