using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Models;

namespace NetReact.ServerManagementService.DB;

public class ApplicationContext : DbContext
{
    public DbSet<ServerDetails> Servers => Set<ServerDetails>();
    public DbSet<ServerFollower> Followers => Set<ServerFollower>();

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }

    //TODO
    public async Task<bool> GetIsUserFollowingServer(string userId, string serverId)
    {
        var queryString = $"SELECT dbo.GetIsUserFollowing('{userId}', '{serverId}') AS Value";

        return await Database.SqlQuery<int>(FormattableStringFactory.Create(queryString)).FirstOrDefaultAsync() == 1;
    }
}