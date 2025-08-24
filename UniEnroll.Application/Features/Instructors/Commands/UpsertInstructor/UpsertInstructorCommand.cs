
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Instructors.Commands.Common;

namespace UniEnroll.Application.Features.Instructors.Commands.UpsertInstructor;

public sealed record UpsertInstructorCommand(
    string InstructorId,
    string FirstName,
    string LastName,
    string Email
) : IRequest<Result<UpsertInstructorResult>>;
