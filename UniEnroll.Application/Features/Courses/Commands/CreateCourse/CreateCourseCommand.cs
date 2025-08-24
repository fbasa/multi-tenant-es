
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Abstractions;
using UniEnroll.Domain.Courses;
using UniEnroll.Domain.Courses.ValueObjects;

namespace UniEnroll.Application.Features.Courses.Commands.CreateCourse;

public sealed record CreateCourseCommand(string TenantId, string Code, string Title, int Units) : IRequest<Result<string>>;

public sealed class CreateCourseHandler(IRepository<Course> repo, IUnitOfWork uow, IIdGenerator ids) : IRequestHandler<CreateCourseCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateCourseCommand request, CancellationToken ct)
    {
        var id = ids.NewId();
        var course = new Course(id, new CourseCode(request.Code), request.Title, new CreditUnit(request.Units), request.TenantId);
        await repo.AddAsync(course, ct);
        await uow.SaveChangesAsync(ct);
        return Result<string>.Success(id);
    }
}
