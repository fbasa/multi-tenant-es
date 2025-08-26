
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Registrar.Commands;

public sealed record PlaceHoldCommand(string TenantId, string StudentId, string HoldType, string Status) : IRequest<Result<string>>;

public sealed class PlaceHoldHandler : IRequestHandler<PlaceHoldCommand, Result<string>>
{
    public Task<Result<string>> Handle(PlaceHoldCommand request, CancellationToken ct)
        => Task.FromResult(Result<string>.Success($"hold-{Guid.NewGuid():N}"));
}
