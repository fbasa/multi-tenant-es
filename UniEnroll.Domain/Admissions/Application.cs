
using UniEnroll.Domain.Common;

namespace UniEnroll.Domain.Admissions;

public sealed class Application : EntityBase, IAggregateRoot
{
    public string ApplicantUserId { get; private set; }
    public string ProgramId { get; private set; }
    public string TermCode { get; private set; }
    public string TenantId { get; private set; }
    public string Status { get; private set; } = "Submitted";

    public Application(string id, string applicantUserId, string programId, string termCode, string tenantId) : base(id)
    {
        ApplicantUserId = applicantUserId;
        ProgramId = programId;
        TermCode = termCode;
        TenantId = tenantId;
    }
}
