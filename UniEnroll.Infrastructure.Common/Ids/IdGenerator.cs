using UniEnroll.Infrastructure.Common.Abstractions;

namespace UniEnroll.Infrastructure.Common.Ids;

public sealed class IdGenerator : IIdGenerator
{
    public string NewId() => System.Guid.NewGuid().ToString("N");
}
