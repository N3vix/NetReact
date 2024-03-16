using Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RESTfulAPI.DB;

namespace RESTfulAPI.Repositories.MongoDB;

public class ChannelMessagesRepositoryMongoDb : IChannelMessagesRepository
{
    private readonly IMongoDbContext _mongoDbContext;

    public ChannelMessagesRepositoryMongoDb(IMongoDbContext mongoDbContext)
    {
        ArgumentNullException.ThrowIfNull(mongoDbContext);

        _mongoDbContext = mongoDbContext;
    }

    public async Task<string> Add(ChannelMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        message.Id = Guid.NewGuid().ToString();

        await _mongoDbContext.ChannelMessages.InsertOneAsync(message);

        return message.Id;
    }

    public async Task<ChannelMessage> GetById(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        var filter = GetIdFilter(id);
        var message = await _mongoDbContext.ChannelMessages.Find(filter).FirstOrDefaultAsync();

        return message;
    }

    public async Task<IEnumerable<ChannelMessage>> Get(string channelId, int take, int skip)
    {
        ArgumentException.ThrowIfNullOrEmpty(channelId);

        var filter = Builders<ChannelMessage>.Filter.Eq(x => x.ChannelId, channelId);
        var messages = await _mongoDbContext.ChannelMessages
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
        var messages = await _mongoDbContext.ChannelMessages
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
            .Set(x => x.Content, message.Content);

        var result = await _mongoDbContext.ChannelMessages.UpdateOneAsync(filter, update);

        return result.ModifiedCount == 1;
    }

    public async Task<bool> Delete(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        var filter = GetIdFilter(id);
        var result = await _mongoDbContext.ChannelMessages.DeleteOneAsync(filter);

        return result.DeletedCount == 1;
    }

    private FilterDefinition<ChannelMessage> GetIdFilter(string id)
        => Builders<ChannelMessage>.Filter.Eq(x => x.Id, id);
}
