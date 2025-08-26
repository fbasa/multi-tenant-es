using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Api.Auth;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Sections.Commands;
using UniEnroll.Application.Features.Sections.Queries;
using UniEnroll.Contracts.Sections;

namespace UniEnroll.Api.Controllers;

public sealed class SectionsController : BaseApiController
{
    public SectionsController(ISender sender) : base(sender) { }

    [HttpPost("{tenantId}")]
    [Authorize(Policy = Policies.Instructor.ManageSections)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromRoute] string tenantId, [FromBody] CreateSectionCommand body, CancellationToken ct)
        => Ok((await Sender.Send(body with { TenantId = tenantId }, ct)).Value);

    [HttpGet("{tenantId}")]
    [ProducesResponseType(typeof(PagedResult<SectionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List([FromRoute] string tenantId, [FromQuery] string? courseId, [FromQuery] string? instructorId, [FromQuery] string? termId, [FromQuery] int page = 1, [FromQuery] int size = 50, CancellationToken ct = default)
        => Ok((await Sender.Send(new ListSectionsQuery(tenantId, courseId, instructorId, termId, page, size), ct)).Value);

    //TODO:
    [HttpPut("{id}/assign-instructor")]
    [Authorize(Policy = Policies.Instructor.ManageSections)]
    public Task<IActionResult> AssignInstructor(/*…*/) => null;
}
