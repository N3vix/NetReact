using Microsoft.EntityFrameworkCore;
using Models;

namespace RESTfulAPI;

public class ServersGateway : IServersGateway
{
    private ServersContext ServersContext { get; }

    public ServersGateway(ServersContext serversContext)
    {
        ServersContext = serversContext ?? throw new ArgumentNullException(nameof(serversContext));
    }

    public async Task Add(params ServerDetails[] serverDetails)
    {
        ArgumentNullException.ThrowIfNull(serverDetails);

        await ServersContext.ServersDetails.AddRangeAsync(serverDetails);
        await ServersContext.SaveChangesAsync();
    }

    public async Task<ServerDetails[]> GetAll()
    {
        return await ServersContext.ServersDetails.ToArrayAsync();
    }

    public async Task<ServerDetails[]> GetByServerId(string[] ids)
    {
        ArgumentNullException.ThrowIfNull(ids);

        return await ServersContext.ServersDetails.Where(x => ids.Contains(x.ServerId)).ToArrayAsync();
    }

    public async Task<ServerDetails> GetByServerId(string id)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(id);

        return await ServersContext.ServersDetails.FirstOrDefaultAsync(x => x.ServerId.Equals(id));
    }

    public async Task Edit(string id, Action<ServerDetails> editor)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(id);
        ArgumentNullException.ThrowIfNull(editor);

        var details = await GetByServerId(id);
        if (details == null) return;
        editor(details);
        await ServersContext.SaveChangesAsync();
    }
}
