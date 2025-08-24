
using FluentValidation;

namespace UniEnroll.Application.Features.Instructors.Commands.UpsertInstructor;

public sealed class UpsertInstructorCommandValidator : AbstractValidator<UpsertInstructorCommand>
{
    public UpsertInstructorCommandValidator()
    {
        RuleFor(x => x.InstructorId).NotEmpty().MaximumLength(64);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
    }
}
