
using UniEnroll.Domain.Common;

namespace UniEnroll.Domain.Tenancy;

public sealed class Tenant : EntityBase, IAggregateRoot
{
    public string Name { get; private set; }
    public string PartitionKey { get; private set; }

    public Tenant(string id, string name, string partitionKey) : base(id)
    {
        Name = name;
        PartitionKey = string.IsNullOrWhiteSpace(partitionKey) ? id : partitionKey;
    }
}
