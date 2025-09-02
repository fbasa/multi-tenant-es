using MediatR;
using UniEnroll.Api.Auth;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Contracts.Courses;
using UniEnroll.Application.Common;
using Microsoft.AspNetCore.Authorization;
using UniEnroll.Application.Features.Courses.Commands;
using UniEnroll.Application.Features.Courses.Queries;

namespace UniEnroll.Api.Controllers;
public sealed class CoursesController : BaseApiController
{
    public CoursesController(ISender sender) : base(sender) { }

    [HttpPost("{tenantId}")]
    [Authorize(Policy = Policies.Registrar.FullAccess)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromRoute] string tenantId, [FromBody] CreateCourseRequest body, CancellationToken ct)
    {
        return Ok((await Sender.Send(new CreateCourseCommand(body with { TenantId = tenantId }), ct)).Value);
    }

    [HttpGet("{tenantId}")]
    [ProducesResponseType(typeof(PagedResult<CourseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List([FromRoute] string tenantId, [FromQuery] int page = 1, [FromQuery] int size = 50, CancellationToken ct = default)
    {
        return Ok((await Sender.Send(new ListCoursesQuery(tenantId, page, size), ct)).Value);
    }
}
