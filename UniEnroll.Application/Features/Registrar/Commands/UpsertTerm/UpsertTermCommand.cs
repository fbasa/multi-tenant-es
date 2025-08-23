
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Registrar.Commands.UpsertTerm;

public sealed record UpsertTermCommand(string TenantId, string? Id, string YearTermCode, string Status) : IRequest<Result<string>>;

public sealed class UpsertTermHandler : IRequestHandler<UpsertTermCommand, Result<string>>
{
    public Task<Result<string>> Handle(UpsertTermCommand request, CancellationToken ct)
        => Task.FromResult(Result<string>.Success(request.Id ?? $"term-{Guid.NewGuid():N}"));
}
