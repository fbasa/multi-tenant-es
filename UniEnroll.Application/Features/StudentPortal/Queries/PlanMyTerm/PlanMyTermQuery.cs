
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.StudentPortal;

namespace UniEnroll.Application.Features.StudentPortal.Queries.PlanMyTerm;

public sealed record PlanMyTermQuery(string TenantId, string StudentId, string TermId) : IRequest<Result<IReadOnlyList<PlanMyTermSuggestionDto>>>;

public sealed class PlanMyTermHandler : IRequestHandler<PlanMyTermQuery, Result<IReadOnlyList<PlanMyTermSuggestionDto>>>
{
    public Task<Result<IReadOnlyList<PlanMyTermSuggestionDto>>> Handle(PlanMyTermQuery request, CancellationToken ct)
        => Task.FromResult(Result<IReadOnlyList<PlanMyTermSuggestionDto>>.Success(new List<PlanMyTermSuggestionDto>()));
}
