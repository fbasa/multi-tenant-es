
using FluentValidation;

namespace UniEnroll.Application.Features.Enrollment.Commands.ReserveSeat;

public sealed class ReserveSeatCommandValidator : AbstractValidator<ReserveSeatCommand>
{
    public ReserveSeatCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.SectionId).NotEmpty();
        RuleFor(x => x.StudentId).NotEmpty();
    }
}
