using Microsoft.EntityFrameworkCore;
using Models;

namespace RESTfulAPI.DB;

public class MessagesContext : ApplicationContext
{
    public DbSet<ChannelMessage> ChannelMessages => Set<ChannelMessage>();

    public MessagesContext(DbContextOptions<MessagesContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ChannelMessage>().ToTable(nameof(ChannelMessage));
    }
}
