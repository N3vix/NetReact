using Microsoft.EntityFrameworkCore;
using Models;

namespace NetReact.ChannelManagementService.DB;

public class ApplicationContext : DbContext
{
    public DbSet<ChannelDetails> Channels => Set<ChannelDetails>();

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