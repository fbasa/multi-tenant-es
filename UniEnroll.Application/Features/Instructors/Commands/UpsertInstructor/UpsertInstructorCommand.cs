
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Instructors;

namespace UniEnroll.Application.Features.Instructors.Commands.UpsertInstructor;

public sealed record UpsertInstructorCommand(string TenantId, string? Id, string FirstName, string LastName, string Email, string Department) : IRequest<Result<string>>;

public sealed class UpsertInstructorHandler : IRequestHandler<UpsertInstructorCommand, Result<string>>
{
    public Task<Result<string>> Handle(UpsertInstructorCommand request, CancellationToken ct)
        => Task.FromResult(Result<string>.Success(request.Id ?? $"instr-{Guid.NewGuid():N}"));
}
