using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using UniEnroll.Infrastructure.Common.Abstractions;

namespace UniEnroll.Infrastructure.Common.Idempotency;

public sealed class InMemoryIdempotencyStore : IIdempotencyStore
{
    private readonly ConcurrentDictionary<string, (string Hash, DateTimeOffset ExpiresAt)> _store = new();

    public Task<bool> CheckAndRecordAsync(string key, string contentSha256, int ttlMinutes, CancellationToken ct)
    {
        var now = DateTimeOffset.UtcNow;
        // purge occasionally
        foreach (var kv in _store)
        {
            if (kv.Value.ExpiresAt <= now)
                _store.TryRemove(kv.Key, out _);
        }

        if (_store.TryGetValue(key, out var entry) && entry.ExpiresAt > now && entry.Hash == contentSha256)
            return Task.FromResult(true);

        _store[key] = (contentSha256, now.AddMinutes(Math.Max(1, ttlMinutes)));
        return Task.FromResult(false);
    }
}
