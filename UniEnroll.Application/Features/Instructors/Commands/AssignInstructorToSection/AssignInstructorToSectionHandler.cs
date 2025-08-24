
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Instructors.Commands.Common;

namespace UniEnroll.Application.Features.Instructors.Commands.AssignInstructorToSection;

public sealed class AssignInstructorToSectionHandler
    : IRequestHandler<AssignInstructorToSectionCommand, Result<AssignInstructorResult>>
{
    private readonly IInstructorCommandRepository _repo;

    public AssignInstructorToSectionHandler(IInstructorCommandRepository repo) => _repo = repo;

    public async Task<Result<AssignInstructorResult>> Handle(AssignInstructorToSectionCommand request, CancellationToken ct)
    {
        var res = await _repo.AssignInstructorToSectionAsync(request.SectionId, request.InstructorId, ct);
        return Result<AssignInstructorResult>.Success(res);
    }
}
