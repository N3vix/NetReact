using Models;
using MongoDB.Driver;
using NetReact.MessagingService.DB;

namespace NetReact.MessagingService.Repositories;

public class MessagesRepository : IMessagesRepository
{
    private readonly IMongoDbContext _mongoDbContext;

    public MessagesRepository(IMongoDbContext mongoDbContext)
    {
        ArgumentNullException.ThrowIfNull(mongoDbContext);

        _mongoDbContext = mongoDbContext;
    }

    public async Task<string> Add(ChannelMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        message.Id = Guid.NewGuid().ToString();

        await _mongoDbContext.Messages.InsertOneAsync(message);

        return message.Id;
    }

    public async Task<ChannelMessage> GetById(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        var filter = GetIdFilter(id);
        var message = await _mongoDbContext.Messages.Find(filter).FirstOrDefaultAsync();

        return message;
    }

    public async Task<IEnumerable<ChannelMessage>> Get(string channelId, int take, int skip)
    {
        ArgumentException.ThrowIfNullOrEmpty(channelId);

        var filter = Builders<ChannelMessage>.Filter.Eq(x => x.ChannelId, channelId);
        var messages = await _mongoDbContext.Messages
            .Find(filter)
            .Limit(take)
            .SortByDescending(x => x.Timestamp).ToListAsync();

        return messages.OrderBy(x => x.Timestamp);
    }

    public async Task<IEnumerable<ChannelMessage>> GetBefore(DateTime dateTime, string channelId, int take)
    {
        ArgumentException.ThrowIfNullOrEmpty(channelId);

        var filter = Builders<ChannelMessage>.Filter.Eq(x => x.ChannelId, channelId);
        filter &= Builders<ChannelMessage>.Filter.Lt(x => x.Timestamp, dateTime);
        var messages = await _mongoDbContext.Messages
            .Find(filter)
            .Limit(take)
            .SortByDescending(x => x.Timestamp).ToListAsync();

        return messages.OrderBy(x => x.Timestamp);
    }

    public async Task<bool> Edit(string id, ChannelMessage message)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        ArgumentNullException.ThrowIfNull(message);

        var filter = GetIdFilter(id);
        var update = Builders<ChannelMessage>.Update
            .Set(x => x.Content, message.Content)
            .Set(x => x.EditedTimestamp, message.EditedTimestamp);

        var result = await _mongoDbContext.Messages.UpdateOneAsync(filter, update);

        return result.ModifiedCount == 1;
    }

    public async Task<bool> Delete(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        var filter = GetIdFilter(id);
        var result = await _mongoDbContext.Messages.DeleteOneAsync(filter);

        return result.DeletedCount == 1;
    }

    private FilterDefinition<ChannelMessage> GetIdFilter(string id)
        => Builders<ChannelMessage>.Filter.Eq(x => x.Id, id);
}
