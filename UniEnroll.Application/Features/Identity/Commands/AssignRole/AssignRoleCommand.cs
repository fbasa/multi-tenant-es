
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Domain.Identity;
using UniEnroll.Infrastructure.EF.Repositories.Contracts;

namespace UniEnroll.Application.Features.Identity.Commands;

public sealed record AssignRoleCommand(string TenantId, string UserId, string Role) : IRequest<Result<bool>>;

public sealed class AssignRoleHandler : IRequestHandler<AssignRoleCommand, Result<bool>>
{
    private readonly IRepositoryBase<User> _repo;
    private readonly IUnitOfWork _uow;
    public AssignRoleHandler(IRepositoryBase<User> repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<Result<bool>> Handle(AssignRoleCommand request, CancellationToken ct)
    {
        var user = await _repo.GetAsync(u => u.Id == request.UserId, ct);
        if (user is null) return Result<bool>.Failure("User not found");
        var roles = user.Roles.Distinct().ToList();
        if (!roles.Contains(request.Role)) roles.Add(request.Role);
        // naive: create new entity (assuming mutable for brevity)
        user.GetType().GetProperty("Roles")?.SetValue(user, roles.ToArray());
        await _uow.SaveChangesAsync(ct);
        return Result<bool>.Success(true);
    }
}
