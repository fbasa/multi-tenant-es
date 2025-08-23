
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Registrar;

namespace UniEnroll.Application.Features.Registrar.Queries.GetGraduationAudit;

public sealed record GetGraduationAuditQuery(string TenantId, string StudentId) : IRequest<Result<GraduationAuditDto>>;

public sealed class GetGraduationAuditHandler : IRequestHandler<GetGraduationAuditQuery, Result<GraduationAuditDto>>
{
    public Task<Result<GraduationAuditDto>> Handle(GetGraduationAuditQuery request, CancellationToken ct)
        => Task.FromResult(Result<GraduationAuditDto>.Success(new GraduationAuditDto(request.StudentId, false, Array.Empty<string>())));
}
