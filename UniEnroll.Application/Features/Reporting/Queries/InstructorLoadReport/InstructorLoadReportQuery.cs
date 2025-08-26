
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Reporting;

namespace UniEnroll.Application.Features.Reporting.Queries;

public sealed record InstructorLoadReportQuery(string TenantId, string InstructorId) : IRequest<Result<IReadOnlyList<InstructorLoadReportRowDto>>>;

public sealed class InstructorLoadReportHandler : IRequestHandler<InstructorLoadReportQuery, Result<IReadOnlyList<InstructorLoadReportRowDto>>>
{
    public Task<Result<IReadOnlyList<InstructorLoadReportRowDto>>> Handle(InstructorLoadReportQuery request, CancellationToken ct)
        => Task.FromResult(Result<IReadOnlyList<InstructorLoadReportRowDto>>.Success(new List<InstructorLoadReportRowDto>()));
}
