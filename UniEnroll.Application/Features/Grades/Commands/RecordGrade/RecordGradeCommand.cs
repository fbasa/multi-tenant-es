
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Grades.Commands;

public sealed record RecordGradeCommand(string TenantId, string EnrollmentId, string Grade) : IRequest<Result<bool>>;

public sealed class RecordGradeHandler : IRequestHandler<RecordGradeCommand, Result<bool>>
{
    public Task<Result<bool>> Handle(RecordGradeCommand request, CancellationToken ct)
        => Task.FromResult(Result<bool>.Success(true));
}
