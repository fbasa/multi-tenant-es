
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Registrar.Commands;

public sealed record RunGraduationAuditCommand(string TenantId, string StudentId) : IRequest<Result<bool>>;

public sealed class RunGraduationAuditHandler : IRequestHandler<RunGraduationAuditCommand, Result<bool>>
{
    public Task<Result<bool>> Handle(RunGraduationAuditCommand request, CancellationToken ct)
        => Task.FromResult(Result<bool>.Success(true));
}
