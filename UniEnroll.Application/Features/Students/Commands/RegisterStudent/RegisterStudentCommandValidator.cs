
using FluentValidation;

namespace UniEnroll.Application.Features.Students.Commands.RegisterStudent;

public sealed class RegisterStudentCommandValidator : AbstractValidator<RegisterStudentCommand>
{
    public RegisterStudentCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.ProgramId).NotEmpty();
        RuleFor(x => x.EntryYear).InclusiveBetween(1900, 2100);
    }
}
