using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Application.Features.Courses.Commands;
using UniEnroll.Application.Features.Courses.Queries;
using UniEnroll.Contracts.Courses;

namespace UniEnroll.Api.Controllers;
public sealed class CoursesController : BaseApiController
{
    public CoursesController(ISender sender) : base(sender) { }

    [HttpPost("{tenantId}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromRoute] string tenantId, [FromBody] CreateCourseRequest body, CancellationToken ct)
        => Ok((await Sender.Send(new CreateCourseCommand(body with { TenantId = tenantId }), ct)).Value);

    [HttpGet("{tenantId}")]
    [ProducesResponseType(typeof(UniEnroll.Application.Common.PagedResult<CourseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List([FromRoute] string tenantId, [FromQuery] int page = 1, [FromQuery] int size = 50, CancellationToken ct = default)
        => Ok((await Sender.Send(new ListCoursesQuery(tenantId, page, size), ct)).Value);
}
