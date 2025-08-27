using System.Threading;
using System.Threading.Tasks;
using UniEnroll.Infrastructure.EF.Persistence;
using UniEnroll.Infrastructure.EF.Repositories.Contracts;

namespace UniEnroll.Infrastructure.EF.Repositories;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly UniEnrollDbContext _db;
    public UnitOfWork(UniEnrollDbContext db) => _db = db;
    public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await _db.SaveChangesAsync(ct);
}
