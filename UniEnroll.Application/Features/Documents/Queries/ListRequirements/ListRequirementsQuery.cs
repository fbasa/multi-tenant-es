
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Documents;

namespace UniEnroll.Application.Features.Documents.Queries;

public sealed record ListRequirementsQuery(string TenantId, string StudentId) : IRequest<Result<IReadOnlyList<RequirementDto>>>;

public sealed class ListRequirementsHandler : IRequestHandler<ListRequirementsQuery, Result<IReadOnlyList<RequirementDto>>>
{
    public Task<Result<IReadOnlyList<RequirementDto>>> Handle(ListRequirementsQuery request, CancellationToken ct)
        => Task.FromResult(Result<IReadOnlyList<RequirementDto>>.Success(new List<RequirementDto>()));
}
