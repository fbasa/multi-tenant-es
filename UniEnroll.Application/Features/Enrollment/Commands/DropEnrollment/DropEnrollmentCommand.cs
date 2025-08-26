
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Abstractions;

namespace UniEnroll.Application.Features.Enrollment.Commands;

public sealed record DropEnrollmentCommand() : IRequest<Result<bool>>
{
    //string TenantId, string EnrollmentId, string SectionId, byte[] RowVersion, string? Reason
    public string TenantId { get; set; }
    public Guid EnrollmentId { get; set; }
    public string SectionId { get; set; }
    public byte[] RowVersion { get; set; }
    public string Reason { get; set; }
};

public sealed class DropEnrollmentHandler : IRequestHandler<DropEnrollmentCommand, Result<bool>>
{
    private readonly IEnrollmentCommandRepository _sql;
    private readonly ICurrentUser _me;
    public DropEnrollmentHandler(IEnrollmentCommandRepository sql, ICurrentUser me) { _sql = sql; _me = me; }

    public async Task<Result<bool>> Handle(DropEnrollmentCommand request, CancellationToken ct)
    {
        var actor = _me.UserId ?? "system";
        await _sql.DropAsync(request.EnrollmentId, "", ct);
        return Result<bool>.Success(true);
    }
}
