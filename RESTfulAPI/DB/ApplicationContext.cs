using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace RESTfulAPI;

public class ApplicationContext : IdentityDbContext
{
    public DbSet<ServerDetails> ServersDetails => Set<ServerDetails>();
    public DbSet<ChannelDetails> ChannelsDetails => Set<ChannelDetails>();

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ServerDetails>().ToTable(nameof(ServerDetails));
        modelBuilder.Entity<ChannelDetails>().ToTable(nameof(ChannelDetails));
        modelBuilder.Entity<ChannelMessage>().ToTable(nameof(ChannelMessage));
    }
}
