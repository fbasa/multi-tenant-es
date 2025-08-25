
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Api.Auth;
using UniEnroll.Application.Features.Instructors.Commands.Common;
using UniEnroll.Application.Features.Instructors.Commands.AssignInstructorToSection;
using UniEnroll.Application.Features.Instructors.Commands.UpsertInstructor;
using UniEnroll.Contracts.Instructors;
using Asp.Versioning;

namespace UniEnroll.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/instructors")]
public sealed class InstructorsController : ControllerBase
{
    private readonly IMediator _mediator;
    public InstructorsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [Authorize(Policy = Policies.Instructor.ManageSections)]
    public async Task<IActionResult> Upsert([FromBody] UpsertInstructorRequest req, CancellationToken ct)
    {
        var r = await _mediator.Send(new UpsertInstructorCommand(req.InstructorId, req.FirstName, req.LastName, req.Email), ct);
        return r.Value.Outcome switch
        {
            InstructorOutcome.Inserted => CreatedAtAction(nameof(Get), new { id = req.InstructorId, version = "1.0" }, new { id = req.InstructorId }),
            InstructorOutcome.Updated  => Ok(new { id = req.InstructorId }),
            _ => StatusCode(409, new ProblemDetails { Title = "Concurrency conflict" })
        };
    }

    // Optional placeholder for lookup (not implemented here)
    [HttpGet("{id}")]
    [Authorize(Policy = Policies.Instructor.ViewLoads)]
    public IActionResult Get([FromRoute] string id) => Ok(new { id });

    [HttpPost("{id}/assign-section")]
    [Authorize(Policy = Policies.Instructor.ManageSections)]
    public async Task<IActionResult> Assign([FromRoute] string id, [FromBody] AssignInstructorToSectionRequest req, CancellationToken ct)
    {
        if (req.InstructorId != null && req.InstructorId != id)
            return BadRequest(new ProblemDetails { Title = "InstructorId mismatch" });

        var r = await _mediator.Send(new AssignInstructorToSectionCommand(req.SectionId, id), ct);
        return r.Value?.Outcome switch
        {
            InstructorOutcome.Assigned         => Ok(),
            InstructorOutcome.ValidationFailed => Conflict(new ProblemDetails { Title = "Schedule conflict for instructor" }),
            InstructorOutcome.NotFound         => NotFound(),
            _                                  => StatusCode(409, new ProblemDetails { Title = "Concurrency conflict" })
        };
    }
}
