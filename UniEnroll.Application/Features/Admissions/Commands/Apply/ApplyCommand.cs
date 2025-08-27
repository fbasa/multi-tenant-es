
using FluentValidation;
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Infrastructure.Common.Abstractions;
using UniEnroll.Infrastructure.EF.Repositories.Contracts;

namespace UniEnroll.Application.Features.Admissions.Commands;

public sealed record ApplyCommand(string TenantId, string ApplicantUserId, string ProgramId, string TermCode)
    : IRequest<Result<string>>;

public sealed class ApplyCommandValidator : AbstractValidator<ApplyCommand>
{
    public ApplyCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.ApplicantUserId).NotEmpty();
        RuleFor(x => x.ProgramId).NotEmpty();
        RuleFor(x => x.TermCode).NotEmpty().MaximumLength(16);
    }
}

public sealed class ApplyCommandHandler(IRepositoryBase<Domain.Admissions.Application> repo, IIdGenerator ids, IUnitOfWork uow) : IRequestHandler<ApplyCommand, Result<string>>
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
