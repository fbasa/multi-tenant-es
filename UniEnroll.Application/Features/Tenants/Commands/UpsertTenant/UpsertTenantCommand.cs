
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Tenants.Commands.UpsertTenant;

public sealed record UpsertTenantCommand(string? Id, string Name, string PartitionKey) : IRequest<Result<string>>;
