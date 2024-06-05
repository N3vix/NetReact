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
}