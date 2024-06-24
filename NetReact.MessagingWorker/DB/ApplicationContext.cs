using Microsoft.EntityFrameworkCore;
using Models;

namespace NetReact.MessagingWorker.DB;

public class ApplicationContext : DbContext
{
    public DbSet<ChannelMessage> Messages => Set<ChannelMessage>();

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }
}