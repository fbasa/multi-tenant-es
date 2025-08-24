
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Scheduling.Commands.Common;

namespace UniEnroll.Application.Features.Scheduling.Commands.BuildTimetable;

public sealed class BuildTimetableCommandHandler : IRequestHandler<BuildTimetableCommand, Result<BuildTimetableResult>>
{
    private readonly ISchedulingRepository _repo;
    public BuildTimetableCommandHandler(ISchedulingRepository repo) => _repo = repo;

    public async Task<Result<BuildTimetableResult>> Handle(BuildTimetableCommand request, CancellationToken ct)
        => Result<BuildTimetableResult>.Success(await _repo.BuildTimetableAsync(request.StudentId, request.TermId, ct));
}
