
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Scheduling.Commands.BuildTimetable;

public sealed record BuildTimetableCommand(string TenantId, string TermId) : IRequest<Result<bool>>;

public sealed class BuildTimetableHandler : IRequestHandler<BuildTimetableCommand, Result<bool>>
{
    public Task<Result<bool>> Handle(BuildTimetableCommand request, CancellationToken ct)
        => Task.FromResult(Result<bool>.Success(true));
}
