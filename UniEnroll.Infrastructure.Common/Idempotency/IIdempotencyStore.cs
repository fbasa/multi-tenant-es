using System.Threading;
using System.Threading.Tasks;

namespace UniEnroll.Infrastructure.Common.Idempotency;

public interface IIdempotencyStore
{
    /// <summary>
    /// Returns true if a matching request (same key & content hash) has already been processed recently.
    /// Otherwise records the hash and returns false.
    /// </summary>
    Task<bool> CheckAndRecordAsync(string key, string contentSha256, int ttlMinutes, CancellationToken ct);
}
