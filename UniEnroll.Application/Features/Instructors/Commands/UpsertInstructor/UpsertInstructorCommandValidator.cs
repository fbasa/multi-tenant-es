
using FluentValidation;

namespace UniEnroll.Application.Features.Instructors.Commands.UpsertInstructor;

public sealed class UpsertInstructorCommandValidator : AbstractValidator<UpsertInstructorCommand>
{
    public UpsertInstructorCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Department).NotEmpty();
    }
}
