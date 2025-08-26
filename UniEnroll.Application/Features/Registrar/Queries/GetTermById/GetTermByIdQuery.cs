
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Registrar;

namespace UniEnroll.Application.Features.Registrar.Queries;

public sealed record GetTermByIdQuery(string TenantId, string TermId) : IRequest<Result<TermDto>>;

public sealed class GetTermByIdHandler : IRequestHandler<GetTermByIdQuery, Result<TermDto>>
{
    public Task<Result<TermDto>> Handle(GetTermByIdQuery request, CancellationToken ct)
        => Task.FromResult(Result<TermDto>.Success(new TermDto(request.TermId, "2025-1", "Open")));
}
