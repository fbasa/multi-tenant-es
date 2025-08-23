
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UniEnroll.Contracts.Reporting;

namespace UniEnroll.Reporting.Abstractions;

public interface IReportStore
{
    Task<IReadOnlyList<EnrollmentReportRowDto>> GetEnrollmentReportAsync(string tenantId, string? termId, CancellationToken ct = default);
    Task<IReadOnlyList<RevenueReportRowDto>> GetRevenueReportAsync(string tenantId, string? termId, CancellationToken ct = default);
    Task<IReadOnlyList<InstructorLoadRowDto>> GetInstructorLoadReportAsync(string tenantId, string? termId, CancellationToken ct = default);
    Task<IReadOnlyList<RetentionCohortRowDto>> GetRetentionCohortReportAsync(string tenantId, int? cohortYear, CancellationToken ct = default);
}
