
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Abstractions;

namespace UniEnroll.Application.Features.Admissions.Commands.Apply;

public sealed record ApplyCommand(string TenantId, string ApplicantUserId, string ProgramId, string TermCode)
    : IRequest<Result<string>>;

public sealed class ApplyCommandHandler(IRepository<Domain.Admissions.Application> repo, IIdGenerator ids, IUnitOfWork uow) : IRequestHandler<ApplyCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ApplyCommand request, CancellationToken ct)
    {
        var id = ids.NewId();
        var app = new Domain.Admissions.Application(id, request.ApplicantUserId, request.ProgramId, request.TermCode, request.TenantId);
        await repo.AddAsync(app, ct);
        await uow.SaveChangesAsync(ct);
        return Result<string>.Success(id);
    }
}
