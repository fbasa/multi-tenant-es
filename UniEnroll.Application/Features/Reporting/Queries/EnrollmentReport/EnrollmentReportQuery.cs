
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Reporting;

namespace UniEnroll.Application.Features.Reporting.Queries.EnrollmentReport;

public sealed record EnrollmentReportQuery(string TenantId, string TermId) : IRequest<Result<IReadOnlyList<EnrollmentReportRowDto>>>;

public sealed class EnrollmentReportHandler : IRequestHandler<EnrollmentReportQuery, Result<IReadOnlyList<EnrollmentReportRowDto>>>
{
    public Task<Result<IReadOnlyList<EnrollmentReportRowDto>>> Handle(EnrollmentReportQuery request, CancellationToken ct)
        => Task.FromResult(Result<IReadOnlyList<EnrollmentReportRowDto>>.Success(new List<EnrollmentReportRowDto>()));
}
