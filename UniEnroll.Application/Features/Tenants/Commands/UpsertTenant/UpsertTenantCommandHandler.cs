
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Abstractions;
using UniEnroll.Domain.Tenancy;

namespace UniEnroll.Application.Features.Tenants.Commands.UpsertTenant;

public sealed class UpsertTenantCommandHandler : IRequestHandler<UpsertTenantCommand, Result<string>>
{
    private readonly IRepository<Tenant> _repo;
    private readonly IUnitOfWork _uow;
    private readonly IIdGenerator _ids;

    public UpsertTenantCommandHandler(IRepository<Tenant> repo, IUnitOfWork uow, IIdGenerator ids)
    { _repo = repo; _uow = uow; _ids = ids; }

    public async Task<Result<string>> Handle(UpsertTenantCommand request, CancellationToken ct)
    {
        var id = request.Id ?? _ids.NewId();
        var tenant = new Tenant(id, request.Name, request.PartitionKey);
        await _repo.AddAsync(tenant, ct);
        await _uow.SaveChangesAsync(ct);
        return Result<string>.Success(id);
    }
}
