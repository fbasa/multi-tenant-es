
using System;

namespace UniEnroll.Domain.Abstractions;

public interface ISoftDelete
{
    bool IsDeleted { get; }
    DateTimeOffset? DeletedAt { get; }
    void MarkDeleted();
}
