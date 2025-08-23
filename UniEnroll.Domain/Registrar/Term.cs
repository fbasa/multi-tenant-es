
using UniEnroll.Domain.Common;

namespace UniEnroll.Domain.Registrar;

public sealed class Term : EntityBase, IAggregateRoot
{
    public string YearTermCode { get; private set; } // e.g., 2025-1
    public string Status { get; private set; } // Open/Closed/etc.
    public string TenantId { get; private set; }

    public Term(string id, string yearTermCode, string status, string tenantId) : base(id)
    {
        YearTermCode = yearTermCode; Status = status; TenantId = tenantId;
    }
}
