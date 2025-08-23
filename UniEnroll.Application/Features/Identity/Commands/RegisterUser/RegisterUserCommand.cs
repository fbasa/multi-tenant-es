
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Abstractions;
using UniEnroll.Domain.Identity;

namespace UniEnroll.Application.Features.Identity.Commands.RegisterUser;

public sealed record RegisterUserCommand(string TenantId, string Email, string[] Roles) : IRequest<Result<string>>;

public sealed class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<string>>
{
    private readonly IRepository<User> _repo;
    private readonly IIdGenerator _ids;
    private readonly IUnitOfWork _uow;
    public RegisterUserHandler(IRepository<User> repo, IIdGenerator ids, IUnitOfWork uow)
    { _repo = repo; _ids = ids; _uow = uow; }

    public async Task<Result<string>> Handle(RegisterUserCommand request, CancellationToken ct)
    {
        var id = _ids.NewId();
        var user = new User(id, request.Email, request.Roles, request.TenantId);
        await _repo.AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct);
        return Result<string>.Success(id);
    }
}
