using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Application.Features.Students.Commands;
using UniEnroll.Application.Features.Students.Queries;
using UniEnroll.Contracts.Students;

namespace UniEnroll.Api.Controllers;

public sealed class StudentsController : BaseApiController
{
    public StudentsController(ISender sender) : base(sender) { }

    [HttpPost("{tenantId}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromRoute] string tenantId, [FromBody] RegisterStudentCommand body, CancellationToken ct)
        => Ok((await Sender.Send(body with { TenantId = tenantId }, ct)).Value);

    [HttpPut("{tenantId}/{studentId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateProfile([FromRoute] string tenantId, [FromRoute] string studentId, [FromBody] UpdateStudentProfileCommand body, CancellationToken ct)
        => Ok(await Sender.Send(body with { TenantId = tenantId, StudentId = studentId }, ct));

    [HttpPut("{tenantId}/{studentId}/prefs")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdatePrefs([FromRoute] string tenantId, [FromRoute] string studentId, [FromBody] UpdateNotificationPrefsCommand body, CancellationToken ct)
        => Ok(await Sender.Send(body with { TenantId = tenantId, StudentId = studentId }, ct));

    [HttpGet("{tenantId}/{studentId}")]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromRoute] string tenantId, [FromRoute] string studentId, CancellationToken ct)
        => Ok((await Sender.Send(new GetStudentByIdQuery(tenantId, studentId), ct)).Value);
}
