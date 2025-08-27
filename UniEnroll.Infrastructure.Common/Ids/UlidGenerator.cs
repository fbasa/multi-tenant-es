
using System;
using UniEnroll.Infrastructure.Common.Abstractions;

namespace UniEnroll.Infrastructure.Common.Ids;

/// <summary>ULID-like sortable id using time prefix + random suffix (not a standard ULID).</summary>
public sealed class UlidGenerator : IIdGenerator
{
    public string NewId()
    {
        var ts = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString("x");
        var rnd = Guid.NewGuid().ToString("N")[..16];
        return $"{ts}{rnd}";
    }
}
