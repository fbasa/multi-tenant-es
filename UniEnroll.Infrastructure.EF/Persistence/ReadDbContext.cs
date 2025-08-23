
using Microsoft.EntityFrameworkCore;

namespace UniEnroll.Infrastructure.EF.Persistence;

public sealed class ReadDbContext : DbContext
{
    public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options) { }
}
