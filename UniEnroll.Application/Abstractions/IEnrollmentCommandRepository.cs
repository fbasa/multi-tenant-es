
using System.Threading;
using System.Threading.Tasks;

namespace UniEnroll.Application.Abstractions;

public interface IEnrollmentCommandRepository
{
    Task EnrollOrWaitlistAsync(string tenantId, string sectionId, string studentId, string enrollmentId, string? reason, CancellationToken ct = default);
    Task DropAsync(string tenantId, string enrollmentId, string sectionId, byte[] rowVersion, string actorUserId, string? reason, CancellationToken ct = default);
}
