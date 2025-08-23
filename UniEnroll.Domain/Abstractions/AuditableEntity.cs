
using System;

namespace UniEnroll.Domain.Abstractions;

public abstract class AuditableEntity : AggregateRoot, ISoftDelete, IRowVersioned
{
    public string? CreatedBy { get; protected set; }
    public string? UpdatedBy { get; protected set; }
    public bool IsDeleted { get; protected set; }
    public DateTimeOffset? DeletedAt { get; protected set; }
    public byte[] RowVersion { get; protected set; } = System.Array.Empty<byte>();

    protected AuditableEntity(string id) : base(id) { }

    public void SetCreatedBy(string userId) => CreatedBy = userId;
    public void SetUpdatedBy(string userId) { UpdatedBy = userId; Touch(); }
    public void MarkDeleted() { IsDeleted = true; DeletedAt = DateTimeOffset.UtcNow; }
}
