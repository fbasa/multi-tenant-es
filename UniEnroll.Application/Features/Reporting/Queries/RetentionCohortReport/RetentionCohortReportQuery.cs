
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Reporting;

namespace UniEnroll.Application.Features.Reporting.Queries;

public sealed record RetentionCohortReportQuery(string TenantId, string CohortYear) : IRequest<Result<IReadOnlyList<RetentionCohortRowDto>>>;

public sealed class RetentionCohortReportHandler : IRequestHandler<RetentionCohortReportQuery, Result<IReadOnlyList<RetentionCohortRowDto>>>
{
    public Task<Result<IReadOnlyList<RetentionCohortRowDto>>> Handle(RetentionCohortReportQuery request, CancellationToken ct)
        => Task.FromResult(Result<IReadOnlyList<RetentionCohortRowDto>>.Success(new List<RetentionCohortRowDto>()));
}
