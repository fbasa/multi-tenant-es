using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Application.Features.Registrar.Commands.ApproveGradeSubmission;
using UniEnroll.Application.Features.Registrar.Commands.ClearHold;
using UniEnroll.Application.Features.Registrar.Commands.PlaceHold;
using UniEnroll.Application.Features.Registrar.Commands.PublishCourseCatalog;
using UniEnroll.Application.Features.Registrar.Commands.RunGraduationAudit;
using UniEnroll.Application.Features.Registrar.Commands.SetEnrollmentWindow;
using UniEnroll.Application.Features.Registrar.Commands.UpsertTerm;
using UniEnroll.Application.Features.Registrar.Queries.GetGraduationAudit;
using UniEnroll.Application.Features.Registrar.Queries.GetTermById;
using UniEnroll.Application.Features.Registrar.Queries.ListActiveHolds;
using UniEnroll.Contracts.Registrar;

namespace UniEnroll.Api.Controllers;

public sealed class RegistrarController : BaseApiController
{
    public RegistrarController(ISender sender) : base(sender) { }

    [HttpPost("{tenantId}/terms")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpsertTerm([FromRoute] string tenantId, [FromBody] UpsertTermCommand body, CancellationToken ct)
        => Ok((await Sender.Send(body with { TenantId = tenantId }, ct)).Value);

    [HttpPost("{tenantId}/publish-catalog")]
    public async Task<IActionResult> PublishCatalog([FromRoute] string tenantId, CancellationToken ct)
        => Ok(await Sender.Send(new PublishCourseCatalogCommand(tenantId), ct));

    [HttpPost("{tenantId}/enrollment-window")]
    public async Task<IActionResult> SetEnrollmentWindow([FromRoute] string tenantId, [FromBody] SetEnrollmentWindowCommand body, CancellationToken ct)
        => Ok(await Sender.Send(body with { TenantId = tenantId }, ct));

    [HttpPost("{tenantId}/holds")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> PlaceHold([FromRoute] string tenantId, [FromBody] PlaceHoldCommand body, CancellationToken ct)
        => Ok((await Sender.Send(body with { TenantId = tenantId }, ct)).Value);

    [HttpDelete("{tenantId}/holds/{holdId}")]
    public async Task<IActionResult> ClearHold([FromRoute] string tenantId, [FromRoute] string holdId, CancellationToken ct)
        => Ok(await Sender.Send(new ClearHoldCommand(tenantId, holdId), ct));

    [HttpPost("{tenantId}/grades/{enrollmentId}/approve")]
    public async Task<IActionResult> ApproveGrade([FromRoute] string tenantId, [FromRoute] string enrollmentId, CancellationToken ct)
        => Ok(await Sender.Send(new ApproveGradeSubmissionCommand(tenantId, enrollmentId), ct));

    [HttpPost("{tenantId}/students/{studentId}/graduation-audit")]
    public async Task<IActionResult> RunGradAudit([FromRoute] string tenantId, [FromRoute] string studentId, CancellationToken ct)
        => Ok(await Sender.Send(new RunGraduationAuditCommand(tenantId, studentId), ct));

    [HttpGet("{tenantId}/terms/{termId}")]
    [ProducesResponseType(typeof(TermDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTerm([FromRoute] string tenantId, [FromRoute] string termId, CancellationToken ct)
        => Ok((await Sender.Send(new GetTermByIdQuery(tenantId, termId), ct)).Value);

    [HttpGet("{tenantId}/students/{studentId}/holds")]
    [ProducesResponseType(typeof(IReadOnlyList<HoldDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListHolds([FromRoute] string tenantId, [FromRoute] string studentId, CancellationToken ct)
        => Ok((await Sender.Send(new ListActiveHoldsQuery(tenantId, studentId), ct)).Value);

    [HttpGet("{tenantId}/students/{studentId}/graduation-audit")]
    [ProducesResponseType(typeof(GraduationAuditDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGradAudit([FromRoute] string tenantId, [FromRoute] string studentId, CancellationToken ct)
        => Ok((await Sender.Send(new GetGraduationAuditQuery(tenantId, studentId), ct)).Value);
}
