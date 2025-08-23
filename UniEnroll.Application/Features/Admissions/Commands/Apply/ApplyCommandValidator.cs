
using FluentValidation;

namespace UniEnroll.Application.Features.Admissions.Commands.Apply;

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
