
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UniEnroll.Infrastructure.EF.Persistence;

public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<UniEnrollDbContext>
{
    public UniEnrollDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<UniEnrollDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=UniEnroll;Trusted_Connection=True;MultipleActiveResultSets=true")
            .Options;
        return new UniEnrollDbContext(options);
    }
}
