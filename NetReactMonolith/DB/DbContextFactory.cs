using Microsoft.EntityFrameworkCore;

namespace NetReactMonolith.DB;

public class DbContextFactory<TContext> : IDbContextFactory<TContext>
    where TContext : DbContext
{
    private IServiceProvider Provider { get; }

    public DbContextFactory(IServiceProvider provider)
    {
        Provider = provider;
    }

    public TContext CreateDbContext()
    {
        if (Provider == null)
            throw new InvalidOperationException($"You must configure an instance of IServiceProvider");

        return ActivatorUtilities.CreateInstance<TContext>(Provider);
    }
}
