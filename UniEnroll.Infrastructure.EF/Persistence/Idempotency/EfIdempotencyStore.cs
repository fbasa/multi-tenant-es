
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniEnroll.Infrastructure.Common.Idempotency;

namespace UniEnroll.Infrastructure.EF.Persistence.Idempotency;

public sealed class EfIdempotencyStore : IIdempotencyStore
{
    private readonly UniEnrollDbContext _db;
    public EfIdempotencyStore(UniEnrollDbContext db) => _db = db;

    public async Task<bool> CheckAndRecordAsync(string key, string contentHash, int ttlMinutes, CancellationToken ct = default)
    {
        var rec = await _db.Set<IdempotencyRecord>().FindAsync(new object[] { key }, ct);
        var now = DateTimeOffset.UtcNow;
        if (rec is not null && rec.Hash == contentHash && rec.ExpiresAt > now) return true;

        if (rec is null)
            _db.Set<IdempotencyRecord>().Add(new IdempotencyRecord { Key = key, Hash = contentHash, ExpiresAt = now.AddMinutes(ttlMinutes) });
        else
        {
            rec.Hash = contentHash;
            rec.ExpiresAt = now.AddMinutes(ttlMinutes);
        }
        await _db.SaveChangesAsync(ct);
        return false;
    }
}
