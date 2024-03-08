using Microsoft.EntityFrameworkCore;
using Models;

namespace RESTfulAPI;

public class ServersContext : ApplicationContext
{
    public DbSet<ServerDetails> ServersDetails => Set<ServerDetails>();

    public ServersContext(DbContextOptions<ServersContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ServerDetails>().ToTable(nameof(ServerDetails));
    }

    public override void Dispose()
    {
        base.Dispose();
    }

    public override ValueTask DisposeAsync()
    {
        return base.DisposeAsync();
    }
}
