
using FluentValidation;

namespace UniEnroll.Application.Features.Scheduling.Commands.AssignRoom;

public sealed class AssignRoomCommandValidator : AbstractValidator<AssignRoomCommand>
{
    public AssignRoomCommandValidator()
    {
        RuleFor(x => x.SectionId).NotEmpty();
        RuleFor(x => x.RoomCode).NotEmpty().MaximumLength(32);
    }
}
