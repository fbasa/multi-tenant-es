using System.Threading;
using System.Threading.Tasks;
using UniEnroll.Application.Abstractions;
using UniEnroll.Infrastructure.EF.Persistence;

namespace UniEnroll.Infrastructure.EF.Repositories;

internal sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly UniEnrollDbContext _db;
    public EfUnitOfWork(UniEnrollDbContext db) => _db = db;
    public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await _db.SaveChangesAsync(ct);
}
