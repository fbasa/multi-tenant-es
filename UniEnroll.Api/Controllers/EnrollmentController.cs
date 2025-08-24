using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Api.Auth;
using UniEnroll.Application.Features.Enrollment.Commands.Common;
using UniEnroll.Application.Features.Enrollment.Commands.EnrollStudent;
using UniEnroll.Application.Features.Enrollment.Commands.DropEnrollment;
using UniEnroll.Application.Features.Enrollment.Commands.ReserveSeat;
using UniEnroll.Contracts.Enrollment;
using Asp.Versioning;

namespace UniEnroll.Api.Controllers;

[Authorize(Policy = Policies.Student.Enroll)]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/enrollment")]
public sealed class EnrollmentController : ControllerBase
{
    private readonly IMediator _mediator;
    public EnrollmentController(IMediator mediator) => _mediator = mediator;

    //[HttpPost("reserve")]
    //public async Task<IActionResult> Reserve([FromBody] ReserveSeatRequest req, CancellationToken ct)
    //{
    //    var cmd = new ReserveSeatCommand(req.SectionId, req.StudentId, HttpContext.TraceIdentifier);
    //    var r = await _mediator.Send(cmd, ct);
    //    return r.Value.Outcome switch
    //    {
    //        EnrollmentOutcome.Enrolled    => Ok(new { outcome = "reserved", reservationId = r.Value.ReservationId, expiresAt = r.Value.ExpiresAt }),
    //        EnrollmentOutcome.Waitlisted  => Accepted(new { outcome = "waitlisted" }),
    //        EnrollmentOutcome.AlreadyEnrolled => Conflict(new ProblemDetails { Title = "Already enrolled" }),
    //        EnrollmentOutcome.NoSeats     => Conflict(new ProblemDetails { Title = "No seats available" }),
    //        EnrollmentOutcome.NotFound    => NotFound(),
    //        _ => StatusCode(409, new ProblemDetails { Title = "Concurrency conflict" })
    //    };
    //}

    //[HttpPost("enroll")]
    //public async Task<IActionResult> Enroll([FromBody] EnrollRequest req, CancellationToken ct)
    //{
    //    var cmd = new EnrollStudentCommand(req.SectionId, req.StudentId, HttpContext.TraceIdentifier);
    //    var r = await _mediator.Send(cmd, ct);
    //    return r.Value.Outcome switch
    //    {
    //        EnrollmentOutcome.Enrolled        => Ok(new { enrollmentId = r.Value.EnrollmentId }),
    //        EnrollmentOutcome.AlreadyEnrolled => Conflict(new ProblemDetails { Title = "Already enrolled" }),
    //        EnrollmentOutcome.NoSeats         => Conflict(new ProblemDetails { Title = "No seats available" }),
    //        _ => StatusCode(409, new ProblemDetails { Title = "Concurrency conflict" })
    //    };
    //}

    //[HttpPost("{enrollmentId:guid}/drop")]
    //public async Task<IActionResult> Drop([FromRoute] Guid enrollmentId, CancellationToken ct)
    //{
    //    var cmd = new DropEnrollmentCommand(enrollmentId, HttpContext.TraceIdentifier);
    //    var r = await _mediator.Send(cmd, ct);
    //    return r.Value.Outcome switch
    //    {
    //        EnrollmentOutcome.Enrolled    => Ok(new { promoted = r.Value.PromotedFromWaitlist }),
    //        EnrollmentOutcome.NotFound    => NotFound(),
    //        _ => StatusCode(409, new ProblemDetails { Title = "Concurrency conflict" })
    //    };
    //}
}
