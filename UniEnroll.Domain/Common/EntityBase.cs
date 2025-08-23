
using System;

namespace UniEnroll.Domain.Common;

public abstract class EntityBase
{
    public string Id { get; protected set; }
    public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; protected set; }
    protected EntityBase(string id) => Id = id;
    public void Touch() => UpdatedAt = DateTimeOffset.UtcNow;
}
