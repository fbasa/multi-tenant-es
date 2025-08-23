
using System;

namespace UniEnroll.Domain.Abstractions;

public abstract class Entity
{
    public string Id { get; protected set; }
    public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; protected set; }
    protected Entity(string id) => Id = id;
    public void Touch() => UpdatedAt = DateTimeOffset.UtcNow;
}
