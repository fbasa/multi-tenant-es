using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Application.Features.Reporting.Queries.EnrollmentReport;
using UniEnroll.Application.Features.Reporting.Queries.InstructorLoadReport;
using UniEnroll.Application.Features.Reporting.Queries.RetentionCohortReport;
using UniEnroll.Application.Features.Reporting.Queries.RevenueReport;
using UniEnroll.Contracts.Reporting;

namespace UniEnroll.Api.Controllers;

public sealed class ReportsController : BaseApiController
{
    public ReportsController(ISender sender) : base(sender) { }

    [HttpGet("{tenantId}/enrollment")]
    [ProducesResponseType(typeof(IReadOnlyList<EnrollmentReportRowDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Enrollment([FromRoute] string tenantId, [FromQuery] string termId, CancellationToken ct)
        => Ok((await Sender.Send(new EnrollmentReportQuery(tenantId, termId), ct)).Value);

    [HttpGet("{tenantId}/revenue")]
    [ProducesResponseType(typeof(IReadOnlyList<RevenueReportRowDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Revenue([FromRoute] string tenantId, [FromQuery] string termId, CancellationToken ct)
        => Ok((await Sender.Send(new RevenueReportQuery(tenantId, termId), ct)).Value);

    [HttpGet("{tenantId}/instructor-load")]
    [ProducesResponseType(typeof(IReadOnlyList<InstructorLoadReportRowDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> InstructorLoad([FromRoute] string tenantId, [FromQuery] string instructorId, CancellationToken ct)
        => Ok((await Sender.Send(new InstructorLoadReportQuery(tenantId, instructorId), ct)).Value);

    [HttpGet("{tenantId}/retention")]
    [ProducesResponseType(typeof(IReadOnlyList<RetentionCohortRowDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Retention([FromRoute] string tenantId, [FromQuery] string cohortYear, CancellationToken ct)
        => Ok((await Sender.Send(new RetentionCohortReportQuery(tenantId, cohortYear), ct)).Value);
}
