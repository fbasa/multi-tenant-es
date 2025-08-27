
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Students;
using UniEnroll.Domain.Students;
using UniEnroll.Infrastructure.EF.Repositories.Contracts;

namespace UniEnroll.Application.Features.Students.Queries;

public sealed record GetStudentByIdQuery(string TenantId, string StudentId) : IRequest<Result<StudentDto>>;

public sealed class GetStudentByIdHandler : IRequestHandler<GetStudentByIdQuery, Result<StudentDto>>
{
    private readonly IQueryRepository<Student> _q;
    public GetStudentByIdHandler(IQueryRepository<Student> q) => _q = q;

    public async Task<Result<StudentDto>> Handle(GetStudentByIdQuery request, CancellationToken ct)
    {
        var s = await _q.GetAsync(x => x.Id == request.StudentId, ct);
        if (s is null) return Result<StudentDto>.Failure("Not found");
        var dto = new StudentDto(s.Id, "", s.Name.First, s.Name.Last, s.Email, s.ProgramId, s.EntryYear);
        return Result<StudentDto>.Success(dto);
    }
}
