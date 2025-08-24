
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Scheduling.Queries.Common;

namespace UniEnroll.Application.Features.Scheduling.Queries.GetStudentSchedule;

public sealed class GetStudentScheduleQueryHandler : IRequestHandler<GetStudentScheduleQuery, Result<IReadOnlyList<ScheduleEntryDto>>>
{
    private readonly ISchedulingRepository _repo;
    public GetStudentScheduleQueryHandler(ISchedulingRepository repo) => _repo = repo;

    public async Task<Result<IReadOnlyList<ScheduleEntryDto>>> Handle(GetStudentScheduleQuery request, CancellationToken ct)
        => Result<IReadOnlyList<ScheduleEntryDto>>.Success(await _repo.GetStudentScheduleAsync(request.StudentId, request.TermId, ct));
}
