
using UniEnroll.Domain.Common;

namespace UniEnroll.Domain.Registrar;

public sealed class Hold : EntityBase, IAggregateRoot
{
    public string StudentId { get; private set; }
    public string HoldType { get; private set; }
    public string Status { get; private set; } // Active/Cleared
    public string TenantId { get; private set; }

    public Hold(string id, string studentId, string holdType, string status, string tenantId) : base(id)
    {
        StudentId = studentId; HoldType = holdType; Status = status; TenantId = tenantId;
    }

    public void Clear() => Status = "Cleared";
}
