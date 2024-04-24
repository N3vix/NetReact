using Microsoft.EntityFrameworkCore;
using Models;
using RESTfulAPI.DB;

namespace RESTfulAPI.Repositories;

public class ServersRepository : IServersRepository
{
    private ApplicationContext ApplicationContext { get; }

    public ServersRepository(ApplicationContext serversContext)
    {
        ApplicationContext = serversContext ?? throw new ArgumentNullException(nameof(serversContext));
    }

    public async Task<string> Add(ServerDetails serverDetails)
    {
        ArgumentNullException.ThrowIfNull(serverDetails);

        await ApplicationContext.ServersDetails.AddAsync(serverDetails);
        await ApplicationContext.SaveChangesAsync();
        return serverDetails.Id;
    }

    public async Task Add(params ServerDetails[] serverDetails)
    {
        ArgumentNullException.ThrowIfNull(serverDetails);

        await ApplicationContext.ServersDetails.AddRangeAsync(serverDetails);
        await ApplicationContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<ServerDetails>> Get()
    {
        return await ApplicationContext.ServersDetails.ToArrayAsync();
    }

    public async Task<ServerDetails> GetById(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        return await ApplicationContext.ServersDetails.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public async Task<IEnumerable<ServerDetails>> GetByUserId(string userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var userServerIds = from serverUser in ApplicationContext.ServerUsers
                            where serverUser.UserId == userId
                            select serverUser.ServerId;

        return await (from serverDetails in ApplicationContext.ServersDetails
                      where userServerIds.Contains(serverDetails.Id)
                      select serverDetails).ToArrayAsync();
    }

    public async Task<bool> Edit(string id, ServerDetails serverDetails)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        ArgumentNullException.ThrowIfNull(serverDetails);

        var details = await GetById(id);
        if (details == null) return false;
        details.Name = serverDetails.Name;

        await ApplicationContext.SaveChangesAsync();
        return true;
    }

    public Task<bool> Delete(string id)
    {
        throw new NotImplementedException();
    }
}
