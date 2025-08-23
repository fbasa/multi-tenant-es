using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniEnroll.Application.Abstractions;
using UniEnroll.Infrastructure.EF.Persistence;

namespace UniEnroll.Infrastructure.EF.Repositories;

internal sealed class EfQueryRepository<T> : IQueryRepository<T> where T : class
{
    private readonly UniEnrollDbContext _db;
    public EfQueryRepository(UniEnrollDbContext db) => _db = db;

    public Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => _db.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate, ct);

    public Task<List<T>> ListAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default)
        => (predicate is null ? _db.Set<T>().AsNoTracking() : _db.Set<T>().AsNoTracking().Where(predicate)).ToListAsync(ct);
}
