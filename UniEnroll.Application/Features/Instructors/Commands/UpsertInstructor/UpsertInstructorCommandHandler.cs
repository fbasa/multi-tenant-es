
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Instructors.Commands.Common;

namespace UniEnroll.Application.Features.Instructors.Commands.UpsertInstructor;

public sealed class UpsertInstructorCommandHandler
    : IRequestHandler<UpsertInstructorCommand, Result<UpsertInstructorResult>>
{
    private readonly IInstructorCommandRepository _repo;

    public UpsertInstructorCommandHandler(IInstructorCommandRepository repo) => _repo = repo;

    public async Task<Result<UpsertInstructorResult>> Handle(UpsertInstructorCommand request, CancellationToken ct)
    {
        var res = await _repo.UpsertInstructorAsync(request.InstructorId, request.FirstName, request.LastName, request.Email, ct);
        return Result<UpsertInstructorResult>.Success(res);
    }
}
