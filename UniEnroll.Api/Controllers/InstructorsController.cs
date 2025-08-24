using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Api.Auth;
using UniEnroll.Application.Features.Instructors.Commands.AssignInstructorToSection;
using UniEnroll.Application.Features.Instructors.Commands.UpsertInstructor;
using UniEnroll.Application.Features.Instructors.Queries.GetInstructorById;
using UniEnroll.Application.Features.Instructors.Queries.ListInstructorLoad;
using UniEnroll.Contracts.Instructors;

namespace UniEnroll.Api.Controllers;

public sealed class InstructorsController : BaseApiController
{
    public InstructorsController(ISender sender) : base(sender) { }

    [HttpPost("{tenantId}")]
    [Authorize(Policy = Policies.Registrar.ManageInstructors)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Upsert([FromRoute] string tenantId, [FromBody] UpsertInstructorCommand body, CancellationToken ct)
        => Ok((await Sender.Send(body with { TenantId = tenantId }, ct)).Value);

    [HttpPost("{tenantId}/{instructorId}/assign/{sectionId}")]
    public async Task<IActionResult> Assign([FromRoute] string tenantId, [FromRoute] string instructorId, [FromRoute] string sectionId, CancellationToken ct)
        => Ok(await Sender.Send(new AssignInstructorToSectionCommand(tenantId, instructorId, sectionId), ct));

    [HttpGet("{tenantId}/{instructorId}")]
    [ProducesResponseType(typeof(InstructorDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromRoute] string tenantId, [FromRoute] string instructorId, CancellationToken ct)
        => Ok((await Sender.Send(new GetInstructorByIdQuery(tenantId, instructorId), ct)).Value);

    [HttpGet("{tenantId}/{instructorId}/load")]
    [Authorize(Policy = Policies.Instructor.ViewLoads)]
    [ProducesResponseType(typeof(InstructorLoadDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Load([FromRoute] string tenantId, [FromRoute] string instructorId, CancellationToken ct)
        => Ok((await Sender.Send(new ListInstructorLoadQuery(tenantId, instructorId), ct)).Value);
}
