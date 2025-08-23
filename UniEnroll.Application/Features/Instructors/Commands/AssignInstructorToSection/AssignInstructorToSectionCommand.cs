
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Instructors.Commands.AssignInstructorToSection;

public sealed record AssignInstructorToSectionCommand(string TenantId, string InstructorId, string SectionId) : IRequest<Result<bool>>;

public sealed class AssignInstructorToSectionHandler : IRequestHandler<AssignInstructorToSectionCommand, Result<bool>>
{
    public Task<Result<bool>> Handle(AssignInstructorToSectionCommand request, CancellationToken ct)
        => Task.FromResult(Result<bool>.Success(true));
}
