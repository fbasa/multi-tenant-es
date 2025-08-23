
using System.Linq;
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Abstractions;
using UniEnroll.Contracts.Scheduling;
using UniEnroll.Domain.Sections;
using UniEnroll.Domain.Enrollment;

namespace UniEnroll.Application.Features.Scheduling.Queries.GetStudentSchedule;

public sealed record GetStudentScheduleQuery(string TenantId, string StudentId, string TermId) : IRequest<Result<TimetableDto>>;

public sealed class GetStudentScheduleHandler : IRequestHandler<GetStudentScheduleQuery, Result<TimetableDto>>
{
    private readonly IQueryRepository<Domain.Enrollment.Enrollment> _eq;
    private readonly IQueryRepository<Section> _sq;
    public GetStudentScheduleHandler(IQueryRepository<Domain.Enrollment.Enrollment> eq, IQueryRepository<Section> sq) { _eq = eq; _sq = sq; }

    public async Task<Result<TimetableDto>> Handle(GetStudentScheduleQuery request, CancellationToken ct)
    {
        var enrolls = await _eq.ListAsync(e => e.StudentId == request.StudentId, ct);
        var sectionIds = enrolls.Select(e => e.SectionId).ToHashSet();
        var sections = await _sq.ListAsync(s => sectionIds.Contains(s.Id) && s.TermId == request.TermId, ct);
        var entries = sections.Select(s => new TimetableEntryDto(s.Id, "", "", s.MeetingDays, s.StartTime, s.EndTime, s.Room?.Code ?? ""))
            .ToArray();
        return Result<TimetableDto>.Success(new TimetableDto(request.StudentId, request.TermId, entries));
    }
}
