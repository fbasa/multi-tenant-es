
using FluentValidation;
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Domain.Tenancy;
using UniEnroll.Infrastructure.Common.Abstractions;
using UniEnroll.Infrastructure.EF.Repositories.Contracts;

namespace UniEnroll.Application.Features.Tenants.Commands;

public sealed record UpsertTenantCommand(string? Id, string Name, string PartitionKey) : IRequest<Result<string>>;

public sealed class UpsertTenantCommandValidator : AbstractValidator<UpsertTenantCommand>
{
    public UpsertTenantCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(128);
        RuleFor(x => x.PartitionKey).NotEmpty().MaximumLength(64);
    }
}

public sealed class UpsertTenantCommandHandler : IRequestHandler<UpsertTenantCommand, Result<string>>
{
    private readonly IRepositoryBase<Tenant> _repo;
    private readonly IUnitOfWork _uow;
    private readonly IIdGenerator _ids;

    public UpsertTenantCommandHandler(IRepositoryBase<Tenant> repo, IUnitOfWork uow, IIdGenerator ids)
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
