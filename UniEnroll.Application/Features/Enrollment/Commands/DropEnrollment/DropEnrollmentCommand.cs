
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Abstractions;

namespace UniEnroll.Application.Features.Enrollment.Commands.DropEnrollment;

public sealed record DropEnrollmentCommand(string TenantId, string EnrollmentId, string SectionId, byte[] RowVersion, string? Reason) : IRequest<Result<bool>>;

public sealed class DropEnrollmentHandler : IRequestHandler<DropEnrollmentCommand, Result<bool>>
{
    private readonly IEnrollmentCommandRepository _sql;
    private readonly ICurrentUser _me;
    public DropEnrollmentHandler(IEnrollmentCommandRepository sql, ICurrentUser me) { _sql = sql; _me = me; }

    public async Task<Result<bool>> Handle(DropEnrollmentCommand request, CancellationToken ct)
    {
        var actor = _me.UserId ?? "system";
        await _sql.DropAsync(request.TenantId, request.EnrollmentId, request.SectionId, request.RowVersion, actor, request.Reason, ct);
        return Result<bool>.Success(true);
    }
}
