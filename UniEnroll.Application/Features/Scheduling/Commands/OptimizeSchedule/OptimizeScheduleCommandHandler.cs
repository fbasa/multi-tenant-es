
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Scheduling.Commands.Common;

namespace UniEnroll.Application.Features.Scheduling.Commands.OptimizeSchedule;

public sealed class OptimizeScheduleCommandHandler : IRequestHandler<OptimizeScheduleCommand, Result<OptimizeScheduleResult>>
{
    private readonly ISchedulingRepository _repo;
    public OptimizeScheduleCommandHandler(ISchedulingRepository repo) => _repo = repo;

    public async Task<Result<OptimizeScheduleResult>> Handle(OptimizeScheduleCommand request, CancellationToken ct)
        => Result<OptimizeScheduleResult>.Success(await _repo.OptimizeAsync(request.TermId, ct));
}
