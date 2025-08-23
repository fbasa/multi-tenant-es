
using UniEnroll.Domain.Common;

namespace UniEnroll.Domain.Identity;

public sealed class User : EntityBase, IAggregateRoot
{
    public string Email { get; private set; }
    public string[] Roles { get; private set; }
    public string TenantId { get; private set; }

    public User(string id, string email, string[] roles, string tenantId) : base(id)
    {
        Email = email;
        Roles = roles ?? System.Array.Empty<string>();
        TenantId = tenantId;
    }
}
