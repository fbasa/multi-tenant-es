
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Abstractions;

namespace UniEnroll.Application.Features.Admissions.Commands.Apply;

public sealed record ApplyCommand(string TenantId, string ApplicantUserId, string ProgramId, string TermCode)
    : IRequest<Result<string>>;

public sealed class ApplyCommandHandler : IRequestHandler<ApplyCommand, Result<string>>
{
    private readonly IRepository<Domain.Admissions.Application> _repo;
    private readonly IIdGenerator _ids;
    private readonly IUnitOfWork _uow;

    public ApplyCommandHandler(IRepository<Domain.Admissions.Application> repo, IIdGenerator ids, IUnitOfWork uow)
    { _repo = repo; _ids = ids; _uow = uow; }

    public async Task<Result<string>> Handle(ApplyCommand request, CancellationToken ct)
    {
        var id = _ids.NewId();
        var app = new Domain.Admissions.Application(id, request.ApplicantUserId, request.ProgramId, request.TermCode, request.TenantId);
        await _repo.AddAsync(app, ct);
        await _uow.SaveChangesAsync(ct);
        return Result<string>.Success(id);
    }
}
