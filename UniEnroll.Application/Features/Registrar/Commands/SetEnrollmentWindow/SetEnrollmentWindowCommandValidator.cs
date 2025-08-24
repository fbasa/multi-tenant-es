
using FluentValidation;

namespace UniEnroll.Application.Features.Registrar.Commands.SetEnrollmentWindow;

public sealed class SetEnrollmentWindowCommandValidator : AbstractValidator<SetEnrollmentWindowCommand>
{
    public SetEnrollmentWindowCommandValidator()
    {
        //RuleFor(x => x.TermId).NotEmpty();
        //RuleFor(x => x.StartAt).LessThan(x => x.EndAt);
    }
}
