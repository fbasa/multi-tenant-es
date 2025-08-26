
using System;
using System.Threading;
using System.Threading.Tasks;
using UniEnroll.Contracts.Enrollment;

namespace UniEnroll.Application.Abstractions;

public interface IEnrollmentCommandRepository
{
    Task<ReserveSeatResult> ReserveSeatAsync(Guid sectionId, string studentId, string? idempotencyKey, CancellationToken ct);
    Task<EnrollSeatResult> EnrollAsync(Guid sectionId, string studentId, string? idempotencyKey, CancellationToken ct);
    Task<DropResult> DropAsync(Guid enrollmentId, string? idempotencyKey, CancellationToken ct);
}
