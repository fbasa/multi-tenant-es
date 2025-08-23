
using FluentValidation;

namespace UniEnroll.Application.Features.Waitlist.Commands.JoinWaitlist;

public sealed class JoinWaitlistCommandValidator : AbstractValidator<JoinWaitlistCommand>
{
    public JoinWaitlistCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.SectionId).NotEmpty();
        RuleFor(x => x.StudentId).NotEmpty();
    }
}
