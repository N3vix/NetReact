using Microsoft.EntityFrameworkCore;

namespace RESTfulAPI;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions options) 
        : base(options)
    {
        Database.EnsureCreated();
    }
}
