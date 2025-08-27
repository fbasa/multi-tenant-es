
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Courses;
using UniEnroll.Domain.Courses;
using UniEnroll.Infrastructure.EF.Repositories.Contracts;

namespace UniEnroll.Application.Features.Courses.Queries;

public sealed record ListCoursesQuery(string TenantId, int Page, int Size) : IRequest<Result<PagedResult<CourseDto>>>;

public sealed class ListCoursesHandler : IRequestHandler<ListCoursesQuery, Result<PagedResult<CourseDto>>>
{
    private readonly IQueryRepository<Course> _q;
    public ListCoursesHandler(IQueryRepository<Course> q) => _q = q;

    public async Task<Result<PagedResult<CourseDto>>> Handle(ListCoursesQuery request, CancellationToken ct)
    {
        var all = await _q.ListAsync(c => c.TenantId == request.TenantId, ct);
        var items = all.Skip((request.Page - 1) * request.Size).Take(request.Size)
            .Select(c => new CourseDto(c.Id, c.Code.Value, c.Title, c.Units.Value, ""))
            .ToList();
        var page = new PagedResult<CourseDto>(request.Page, request.Size, all.Count, items);
        return Result<PagedResult<CourseDto>>.Success(page);
    }
}
