
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Common.Pagination;
using UniEnroll.Contracts.CourseSearch;

namespace UniEnroll.Application.Features.CourseSearch.Queries;

public sealed record SearchCoursesQuery(CourseSearchRequest Request) : IRequest<Result<KeysetPageResult<CourseSearchResultDto>>>;

public sealed class SearchCoursesHandler : IRequestHandler<SearchCoursesQuery, Result<KeysetPageResult<CourseSearchResultDto>>>
{
    public Task<Result<KeysetPageResult<CourseSearchResultDto>>> Handle(SearchCoursesQuery request, CancellationToken ct)
    {
        var page = new KeysetPageResult<CourseSearchResultDto>(next: null, prev: null, pageSize: request.Request.PageSize, items: Array.Empty<CourseSearchResultDto>());
        return Task.FromResult(Result<KeysetPageResult<CourseSearchResultDto>>.Success(page));
    }
}
