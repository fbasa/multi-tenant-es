
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Registrar.Commands.ApproveGradeSubmission;

public sealed record ApproveGradeSubmissionCommand(string TenantId, string EnrollmentId) : IRequest<Result<bool>>;

public sealed class ApproveGradeSubmissionHandler : IRequestHandler<ApproveGradeSubmissionCommand, Result<bool>>
{
    public Task<Result<bool>> Handle(ApproveGradeSubmissionCommand request, CancellationToken ct)
        => Task.FromResult(Result<bool>.Success(true));
}
