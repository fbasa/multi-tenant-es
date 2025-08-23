
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Scheduling.Commands.OptimizeSchedule;

public sealed record OptimizeScheduleCommand(string TenantId) : IRequest<Result<bool>>;

public sealed class OptimizeScheduleHandler : IRequestHandler<OptimizeScheduleCommand, Result<bool>>
{
    public Task<Result<bool>> Handle(OptimizeScheduleCommand request, CancellationToken ct)
        => Task.FromResult(Result<bool>.Success(true));
}
