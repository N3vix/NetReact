using Models;

namespace NetReact.MessagingWorker.Repositories;

public interface IMessagesRepository
{
    Task<string> Add(ChannelMessage channelDetails);
    Task<IEnumerable<ChannelMessage>> Get(string channelId, int take, DateTime? from);
    Task<ChannelMessage> GetById(string id);
    Task<bool> Edit(string id, Action<ChannelMessage> editor);
    Task<bool> Delete(string id);
    Task Save();
}