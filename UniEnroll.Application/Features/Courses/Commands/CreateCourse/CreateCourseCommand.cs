
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Abstractions;
using UniEnroll.Domain.Courses;
using UniEnroll.Domain.Courses.ValueObjects;

namespace UniEnroll.Application.Features.Courses.Commands.CreateCourse;

public sealed record CreateCourseCommand(string TenantId, string Code, string Title, int Units) : IRequest<Result<string>>;

public sealed class CreateCourseHandler : IRequestHandler<CreateCourseCommand, Result<string>>
{
    private readonly IRepository<Course> _repo;
    private readonly IUnitOfWork _uow;
    private readonly IIdGenerator _ids;
    public CreateCourseHandler(IRepository<Course> repo, IUnitOfWork uow, IIdGenerator ids) { _repo = repo; _uow = uow; _ids = ids; }

    public async Task<Result<string>> Handle(CreateCourseCommand request, CancellationToken ct)
    {
        var id = _ids.NewId();
        var course = new Course(id, new CourseCode(request.Code), request.Title, new CreditUnit(request.Units), request.TenantId);
        await _repo.AddAsync(course, ct);
        await _uow.SaveChangesAsync(ct);
        return Result<string>.Success(id);
    }
}
