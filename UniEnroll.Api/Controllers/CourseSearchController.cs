using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Application.Features.CourseSearch.Queries.SearchCourses;
using UniEnroll.Contracts.CourseSearch;

namespace UniEnroll.Api.Controllers;

public sealed class CourseSearchController : BaseApiController
{
    public CourseSearchController(ISender sender) : base(sender) { }

    [HttpGet("{tenantId}")]
    [ProducesResponseType(typeof(UniEnroll.Application.Common.Pagination.KeysetPageResult<CourseSearchResultDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromRoute] string tenantId, [FromQuery] string? keyword, [FromQuery] string? department, [FromQuery] string? instructorId, [FromQuery] string? dayFilter, [FromQuery] string? timeFrom, [FromQuery] string? timeTo, [FromQuery] int pageSize = 50, [FromQuery] string? next = null, [FromQuery] string? prev = null, CancellationToken ct = default)
    {
        var req = new CourseSearchRequest(tenantId, keyword, department, instructorId, dayFilter, timeFrom, timeTo, pageSize, next, prev);
        return Ok((await Sender.Send(new SearchCoursesQuery(req), ct)).Value);
    }
}
