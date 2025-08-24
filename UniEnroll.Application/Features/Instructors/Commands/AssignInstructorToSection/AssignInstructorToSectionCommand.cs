
using System;
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Instructors.Commands.Common;

namespace UniEnroll.Application.Features.Instructors.Commands.AssignInstructorToSection;

public sealed record AssignInstructorToSectionCommand(
    Guid SectionId,
    string InstructorId
) : IRequest<Result<AssignInstructorResult>>;
