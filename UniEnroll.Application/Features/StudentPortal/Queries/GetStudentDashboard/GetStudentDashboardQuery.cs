
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.StudentPortal;
using UniEnroll.Contracts.Common;

namespace UniEnroll.Application.Features.StudentPortal.Queries.GetStudentDashboard;

public sealed record GetStudentDashboardQuery(string TenantId, string StudentId, string TermId) : IRequest<Result<StudentDashboardDto>>;

public sealed class GetStudentDashboardHandler : IRequestHandler<GetStudentDashboardQuery, Result<StudentDashboardDto>>
{
    public Task<Result<StudentDashboardDto>> Handle(GetStudentDashboardQuery request, CancellationToken ct)
    {
        var dto = new StudentDashboardDto(request.StudentId, request.TermId, 0, 0, new MoneyDto(0m), Array.Empty<string>());
        return Task.FromResult(Result<StudentDashboardDto>.Success(dto));
    }
}
