
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Abstractions;
using UniEnroll.Contracts.Enrollment;
using UniEnroll.Domain.Enrollment;

namespace UniEnroll.Application.Features.Enrollment.Queries.GetEnrollmentById;

public sealed record GetEnrollmentByIdQuery(string TenantId, string EnrollmentId) : IRequest<Result<EnrollmentDto>>;

public sealed class GetEnrollmentByIdHandler : IRequestHandler<GetEnrollmentByIdQuery, Result<EnrollmentDto>>
{
    private readonly IQueryRepository<Domain.Enrollment.Enrollment> _q;
    public GetEnrollmentByIdHandler(IQueryRepository<Domain.Enrollment.Enrollment> q) => _q = q;

    public async Task<Result<EnrollmentDto>> Handle(GetEnrollmentByIdQuery request, CancellationToken ct)
    {
        var e = await _q.GetAsync(x => x.Id == request.EnrollmentId, ct);
        if (e is null) return Result<EnrollmentDto>.Failure("Not found");
        var dto = new EnrollmentDto(e.Id, e.StudentId, e.SectionId, e.Status.ToString(), e.CreatedAt);
        return Result<EnrollmentDto>.Success(dto);
    }
}
