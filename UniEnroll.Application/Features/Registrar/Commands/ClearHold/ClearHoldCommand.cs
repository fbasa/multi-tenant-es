
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Registrar.Commands.ClearHold;

public sealed record ClearHoldCommand(string TenantId, string HoldId) : IRequest<Result<bool>>;

public sealed class ClearHoldHandler : IRequestHandler<ClearHoldCommand, Result<bool>>
{
    public Task<Result<bool>> Handle(ClearHoldCommand request, CancellationToken ct)
        => Task.FromResult(Result<bool>.Success(true));
}
