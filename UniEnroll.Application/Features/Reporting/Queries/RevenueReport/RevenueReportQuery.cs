
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Reporting;
using UniEnroll.Contracts.Common;

namespace UniEnroll.Application.Features.Reporting.Queries.RevenueReport;

public sealed record RevenueReportQuery(string TenantId, string TermId) : IRequest<Result<IReadOnlyList<RevenueReportRowDto>>>;

public sealed class RevenueReportHandler : IRequestHandler<RevenueReportQuery, Result<IReadOnlyList<RevenueReportRowDto>>>
{
    public Task<Result<IReadOnlyList<RevenueReportRowDto>>> Handle(RevenueReportQuery request, CancellationToken ct)
        => Task.FromResult(Result<IReadOnlyList<RevenueReportRowDto>>.Success(new List<RevenueReportRowDto>()));
}
