using Microsoft.EntityFrameworkCore;
using Models;

namespace RESTfulAPI.Gateways;

public class ServersGateway : IServersGateway
{
    private ApplicationContext ApplicationContext { get; }

    public ServersGateway(ApplicationContext serversContext)
    {
        ApplicationContext = serversContext ?? throw new ArgumentNullException(nameof(serversContext));
    }

    public async Task Add(params ServerDetails[] serverDetails)
    {
        ArgumentNullException.ThrowIfNull(serverDetails);

        await ApplicationContext.ServersDetails.AddRangeAsync(serverDetails);
        await ApplicationContext.SaveChangesAsync();
    }

    public async Task<ServerDetails[]> GetAll()
    {
        return await ApplicationContext.ServersDetails.ToArrayAsync();
    }

    public async Task<ServerDetails[]> GetByServerId(string[] ids)
    {
        ArgumentNullException.ThrowIfNull(ids);

        return await ApplicationContext.ServersDetails.Where(x => ids.Contains(x.ServerId)).ToArrayAsync();
    }

    public async Task<ServerDetails> GetByServerId(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        return await ApplicationContext.ServersDetails.FirstOrDefaultAsync(x => x.ServerId.Equals(id));
    }

    public async Task Edit(string id, Action<ServerDetails> editor)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        ArgumentNullException.ThrowIfNull(editor);

        var details = await GetByServerId(id);
        if (details == null) return;
        editor(details);
        await ApplicationContext.SaveChangesAsync();
    }
}
