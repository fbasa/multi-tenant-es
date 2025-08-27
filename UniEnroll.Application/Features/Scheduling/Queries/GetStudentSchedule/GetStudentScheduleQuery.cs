
using MediatR;
using System;
using System.Collections.Generic;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Scheduling;
using UniEnroll.Infrastructure.EF.Repositories.Contracts;

namespace UniEnroll.Application.Features.Scheduling.Queries;

public sealed record GetStudentScheduleQuery(string StudentId, Guid TermId) : IRequest<Result<IReadOnlyList<ScheduleEntryDto>>>;

public sealed class GetStudentScheduleQueryHandler : IRequestHandler<GetStudentScheduleQuery, Result<IReadOnlyList<ScheduleEntryDto>>>
{
    private readonly ISchedulingRepository _repo;
    public GetStudentScheduleQueryHandler(ISchedulingRepository repo) => _repo = repo;

    public async Task<Result<IReadOnlyList<ScheduleEntryDto>>> Handle(GetStudentScheduleQuery request, CancellationToken ct)
        => Result<IReadOnlyList<ScheduleEntryDto>>.Success(await _repo.GetStudentScheduleAsync(request.StudentId, request.TermId, ct));
}

