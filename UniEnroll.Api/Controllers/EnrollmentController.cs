using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Api.Auth;
using UniEnroll.Application.Features.Enrollment.Commands.DropEnrollment;
using UniEnroll.Application.Features.Enrollment.Commands.EnrollStudent;
using UniEnroll.Application.Features.Enrollment.Commands.ReserveSeat;
using UniEnroll.Application.Features.Enrollment.Queries.GetEnrollmentById;
using UniEnroll.Contracts.Enrollment;

namespace UniEnroll.Api.Controllers;

public sealed class EnrollmentController : BaseApiController
{
    public EnrollmentController(ISender sender) : base(sender) { }

    [HttpPost("{tenantId}/reserve")]
    [Authorize(Policy = Policies.Student.Enroll)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Reserve([FromRoute] string tenantId, [FromBody] ReserveSeatCommand body, CancellationToken ct)
        => Ok((await Sender.Send(body with { TenantId = tenantId }, ct)).Value);

    [HttpPost("{tenantId}/enroll")]
    [Authorize(Policy = Policies.Student.Enroll)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Enroll([FromRoute] string tenantId, [FromBody] EnrollStudentCommand body, CancellationToken ct)
        => Ok((await Sender.Send(body with { TenantId = tenantId }, ct)).Value);

    public sealed record DropBody(byte[] RowVersion, string? Reason);

    [HttpPost("{tenantId}/{enrollmentId}/drop")]
    [Authorize(Policy = Policies.Student.Enroll)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Drop([FromRoute] string tenantId, [FromRoute] string enrollmentId, [FromQuery] string sectionId, [FromBody] DropBody body, CancellationToken ct)
        => Ok(await Sender.Send(new DropEnrollmentCommand(tenantId, enrollmentId, sectionId, body.RowVersion, body.Reason), ct));

    [HttpGet("{tenantId}/{enrollmentId}")]
    [ProducesResponseType(typeof(EnrollmentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromRoute] string tenantId, [FromRoute] string enrollmentId, CancellationToken ct)
        => Ok((await Sender.Send(new GetEnrollmentByIdQuery(tenantId, enrollmentId), ct)).Value);
}
