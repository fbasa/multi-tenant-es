
using System;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Api.Auth;
using UniEnroll.Application.Features.Registrar.Commands.Common;
using UniEnroll.Application.Features.Registrar.Commands.SetEnrollmentWindow;
using UniEnroll.Application.Features.Registrar.Commands.UpsertTerm;
using UniEnroll.Contracts.Registrar;

namespace UniEnroll.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/registrar")]
public sealed class RegistrarController : ControllerBase
{
    private readonly IMediator _mediator;
    public RegistrarController(IMediator mediator) => _mediator = mediator;

    [HttpPost("terms")]
    [Authorize(Policy = Policies.Registrar.ManageHolds)]
    public async Task<IActionResult> UpsertTerm([FromBody] UpsertTermRequest req, CancellationToken ct)
    {
        var r = await _mediator.Send(new UpsertTermCommand(req), ct);
        return r.Value?.Outcome switch
        {
            RegistrarOutcome.Inserted => CreatedAtAction(nameof(GetTerm), new { id = req.TermId, version = "1.0" }, new { id = req.TermId }),
            RegistrarOutcome.Updated  => Ok(new { id = req.TermId }),
            RegistrarOutcome.Conflict => StatusCode(409, new ProblemDetails { Title = "Concurrency conflict" }),
            RegistrarOutcome.ValidationFailed => BadRequest(new ProblemDetails { Title = "Validation failed" }),
            _ => StatusCode(500)
        };
    }

    // Optional placeholder for lookup (not implemented here)
    [HttpGet("terms/{id:guid}")]
    public IActionResult GetTerm([FromRoute] Guid id) => Ok(new { id });

    [HttpPost("terms/{id:guid}/enrollment-window")]
    [Authorize(Policy = Policies.Registrar.ManageTerms)]
    public async Task<IActionResult> SetEnrollmentWindow([FromRoute] Guid id, [FromBody] SetEnrollmentWindowRequest req, CancellationToken ct)
    {
        if (req.TermId != id) return BadRequest(new ProblemDetails { Title = "TermId mismatch" });
        var r = await _mediator.Send(new SetEnrollmentWindowCommand(req), ct);
        return r.Value?.Outcome switch
        {
            RegistrarOutcome.Inserted        => Ok(),
            RegistrarOutcome.Updated         => Ok(),
            RegistrarOutcome.NotFound        => NotFound(),
            RegistrarOutcome.ValidationFailed=> BadRequest(new ProblemDetails { Title = "Window must be within term and StartAt < EndAt" }),
            RegistrarOutcome.Conflict        => StatusCode(409, new ProblemDetails { Title = "Concurrency conflict" }),
            _ => StatusCode(500)
        };
    }
}
