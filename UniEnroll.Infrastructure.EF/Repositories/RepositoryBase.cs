using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UniEnroll.Infrastructure.EF.Persistence;
using UniEnroll.Infrastructure.EF.Repositories.Contracts;

namespace UniEnroll.Infrastructure.EF.Repositories;

internal sealed class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly UniEnrollDbContext _db;
    public RepositoryBase(UniEnrollDbContext db) => _db = db;

    public async Task AddAsync(T entity, CancellationToken ct = default)
        => await _db.Set<T>().AddAsync(entity, ct);

    public Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => _db.Set<T>().FirstOrDefaultAsync(predicate, ct);

    public Task<List<T>> ListAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default)
        => (predicate is null ? _db.Set<T>() : _db.Set<T>().Where(predicate)).ToListAsync(ct);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
