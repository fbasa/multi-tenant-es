
using MediatR;
using System;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Scheduling;

namespace UniEnroll.Application.Features.Scheduling.Commands;

public sealed record OptimizeScheduleCommand(Guid? TermId) : IRequest<Result<OptimizeScheduleResult>>;

public sealed class OptimizeScheduleCommandHandler : IRequestHandler<OptimizeScheduleCommand, Result<OptimizeScheduleResult>>
{
    private readonly ISchedulingRepository _repo;
    public OptimizeScheduleCommandHandler(ISchedulingRepository repo) => _repo = repo;

    public async Task<Result<OptimizeScheduleResult>> Handle(OptimizeScheduleCommand request, CancellationToken ct)
        => Result<OptimizeScheduleResult>.Success(await _repo.OptimizeAsync(request.TermId, ct));
}
