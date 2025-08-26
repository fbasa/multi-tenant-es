
using System;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Api.Auth;
using UniEnroll.Application.Features.Scheduling.Commands;
using UniEnroll.Application.Features.Scheduling.Queries;
using UniEnroll.Contracts.Scheduling;

namespace UniEnroll.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/schedules")]
public sealed class SchedulesController : ControllerBase
{
    private readonly IMediator _mediator;
    public SchedulesController(IMediator mediator) => _mediator = mediator;

    [HttpPost("build")]
    [Authorize(Policy = Policies.Scheduling.Build)]
    public async Task<IActionResult> Build([FromBody] BuildTimetableRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(new BuildTimetableCommand(req.StudentId, req.TermId), ct);
        return result.Value?.Outcome switch
        {
            SchedulingOutcome.Success => Ok(new { created = result.Value.EntriesCreated }), 
            _ => StatusCode(409)
        };
    }

    [HttpPost("assign-room")]
    [Authorize(Policy = Policies.Scheduling.AssignRoom)]
    public async Task<IActionResult> AssignRoom([FromBody] AssignRoomRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(new AssignRoomCommand(req.SectionId, req.RoomCode), ct);
        return result.Value?.Outcome switch
        {
            SchedulingOutcome.Success         => Ok(),
            SchedulingOutcome.Conflict        => Conflict(new ProblemDetails { Title = "Room clash" }),
            SchedulingOutcome.NotFound        => NotFound(),
            SchedulingOutcome.ValidationFailed=> BadRequest(),
            _ => StatusCode(409)
        };
    }

    [HttpPost("optimize")]
    [Authorize(Policy = Policies.Scheduling.Optimize)]
    public async Task<IActionResult> Optimize([FromBody] OptimizeScheduleRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(new OptimizeScheduleCommand(req.TermId), ct);
        return Ok(new { conflictsRecorded = result.Value?.ConflictsRecorded });
    }

    [HttpGet("{studentId}/terms/{termId:guid}")]
    [Authorize(Policy = Policies.Scheduling.ViewStudentSchedule)]
    public async Task<IActionResult> GetStudentSchedule([FromRoute] string studentId, [FromRoute] Guid termId, CancellationToken ct)
        => Ok((await _mediator.Send(new GetStudentScheduleQuery(studentId, termId), ct)).Value);

    [HttpGet("room-conflicts/{termId:guid}")]
    [Authorize(Policy = Policies.Scheduling.ViewConflicts)]
    public async Task<IActionResult> ListRoomConflicts([FromRoute] Guid termId, CancellationToken ct)
        => Ok((await _mediator.Send(new ListRoomConflictsQuery(termId), ct)).Value);
}
