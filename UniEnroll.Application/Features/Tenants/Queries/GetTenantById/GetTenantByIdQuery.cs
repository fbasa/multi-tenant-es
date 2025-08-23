
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Domain.Tenancy;

namespace UniEnroll.Application.Features.Tenants.Queries.GetTenantById;

public sealed record GetTenantByIdQuery(string TenantId) : IRequest<Result<Tenant>>;
