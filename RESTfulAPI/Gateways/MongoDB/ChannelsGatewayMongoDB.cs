using Models;
using MongoDB.Driver;
using RESTfulAPI.DB;

namespace RESTfulAPI.Gateways;

public class ChannelsGatewayMongoDB : IChannelsGateway
{
    private readonly IMongoDbContext _mongoDbContext;

    public ChannelsGatewayMongoDB(IMongoDbContext mongoDbContext)
    {
        ArgumentNullException.ThrowIfNull(mongoDbContext);

        _mongoDbContext = mongoDbContext;
    }

    public async Task<string> Add(ChannelDetails channelDetails)
    {
        ArgumentNullException.ThrowIfNull(channelDetails);

        channelDetails.Id = Guid.NewGuid().ToString();

        await _mongoDbContext.Channels.InsertOneAsync(channelDetails);

        return channelDetails.Id;
    }


    public async Task<ChannelDetails> GetById(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        var filter = GetIdFilter(id);
        var channel = await _mongoDbContext.Channels.Find(filter).FirstOrDefaultAsync();

        return channel;
    }

    public async Task<IEnumerable<ChannelDetails>> GetByServerId(string serverId)
    {
        ArgumentException.ThrowIfNullOrEmpty(serverId);

        var filter = Builders<ChannelDetails>.Filter.Eq(x => x.ServerId, serverId);
        var channels = await _mongoDbContext.Channels.Find(filter).ToListAsync();

        return channels;
    }

    public async Task<bool> Edit(string id, ChannelDetails channelDetails)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        ArgumentNullException.ThrowIfNull(channelDetails);

        var filter = GetIdFilter(id);
        var update = Builders<ChannelDetails>.Update
            .Set(x => x.Name, channelDetails.Name)
            .Set(x => x.Type, channelDetails.Type);

        var result = await _mongoDbContext.Channels.UpdateOneAsync(filter, update);

        return result.ModifiedCount == 1;
    }

    public async Task<bool> Delete(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        var filter = GetIdFilter(id);
        var result = await _mongoDbContext.Channels.DeleteOneAsync(filter);

        return result.DeletedCount == 1;
    }

    private FilterDefinition<ChannelDetails> GetIdFilter(string id)
        => Builders<ChannelDetails>.Filter.Eq(x => x.Id, id);
}
