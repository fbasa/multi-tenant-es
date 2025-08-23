
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Registrar;

namespace UniEnroll.Application.Features.Registrar.Queries.ListActiveHolds;

public sealed record ListActiveHoldsQuery(string TenantId, string StudentId) : IRequest<Result<IReadOnlyList<HoldDto>>>;

public sealed class ListActiveHoldsHandler : IRequestHandler<ListActiveHoldsQuery, Result<IReadOnlyList<HoldDto>>>
{
    public Task<Result<IReadOnlyList<HoldDto>>> Handle(ListActiveHoldsQuery request, CancellationToken ct)
        => Task.FromResult(Result<IReadOnlyList<HoldDto>>.Success(new List<HoldDto>()));
}
