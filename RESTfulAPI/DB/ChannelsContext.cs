using Microsoft.EntityFrameworkCore;
using Models;

namespace RESTfulAPI;

public class ChannelsContext : ApplicationContext
{
    public DbSet<ChannelDetails> ChannelsDetails => Set<ChannelDetails>();

    public ChannelsContext(DbContextOptions<ChannelsContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ChannelDetails>().ToTable(nameof(ChannelDetails));
    }
}
