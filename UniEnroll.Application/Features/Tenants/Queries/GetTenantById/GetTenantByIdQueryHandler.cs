
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Abstractions;
using UniEnroll.Domain.Tenancy;

namespace UniEnroll.Application.Features.Tenants.Queries.GetTenantById;

public sealed class GetTenantByIdQueryHandler : IRequestHandler<GetTenantByIdQuery, Result<Tenant>>
{
    private readonly IQueryRepository<Tenant> _q;
    public GetTenantByIdQueryHandler(IQueryRepository<Tenant> q) => _q = q;

    public async Task<Result<Tenant>> Handle(GetTenantByIdQuery request, CancellationToken ct)
    {
        var tenant = await _q.GetAsync(t => t.Id == request.TenantId, ct);
        return tenant is null ? Result<Tenant>.Failure("Not found") : Result<Tenant>.Success(tenant);
    }
}
