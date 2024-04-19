using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts;

public sealed class DatabaseContext(
    DbContextOptions<DatabaseContext> options
    ) : DbContext(options)
{

    override protected void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

    }
}
