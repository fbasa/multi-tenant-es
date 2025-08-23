
using FluentValidation;

namespace UniEnroll.Application.Features.Registrar.Commands.SetEnrollmentWindow;

public sealed class SetEnrollmentWindowCommandValidator : AbstractValidator<SetEnrollmentWindowCommand>
{
    public SetEnrollmentWindowCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.OpensAt).LessThan(x => x.ClosesAt);
    }
}
