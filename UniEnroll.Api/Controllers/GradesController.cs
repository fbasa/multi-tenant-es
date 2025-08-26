using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Api.Auth;
using UniEnroll.Application.Features.Grades.Commands;
using UniEnroll.Application.Features.Grades.Queries.GetTranscript;
using UniEnroll.Contracts.Grades;

namespace UniEnroll.Api.Controllers;

public sealed class GradesController : BaseApiController
{
    public GradesController(ISender sender) : base(sender) { }

    [HttpPost("{tenantId}/record")]
    [Authorize(Policy = Policies.Instructor.RecordGrades)]
    public async Task<IActionResult> Record([FromRoute] string tenantId, [FromBody] RecordGradeRequest request, CancellationToken ct)
    {
        var result = await Sender.Send(new RecordGradeCommand(request), ct);
        return result.Value?.Outcome switch
        {
            GradeOutcome.Inserted => CreatedAtAction("GetGrade", new { id = result.Value.Id }, new { id = result.Value.Id }),
            GradeOutcome.ValidationFailed => Conflict(new ProblemDetails { Title = "Grade conflict" }),
            _ => StatusCode(409, new ProblemDetails { Title = "Try again later."})
        };
    }

    [HttpGet("{tenantId}/students/{studentId}/transcript")]
    [Authorize(Policy = Policies.Registrar.ApproveGrades)]
    [ProducesResponseType(typeof(TranscriptDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTranscript([FromRoute] string tenantId, [FromRoute] string studentId, CancellationToken ct)
        => Ok((await Sender.Send(new GetTranscriptQuery(tenantId, studentId), ct)).Value);
}
