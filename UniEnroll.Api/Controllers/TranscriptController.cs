using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Application.Features.Transcript.Commands.RequestTranscript;
using UniEnroll.Application.Features.Transcript.Queries.GetTranscriptRequest;
using UniEnroll.Contracts.Transcript;

namespace UniEnroll.Api.Controllers;

public sealed class TranscriptController : BaseApiController
{
    public TranscriptController(ISender sender) : base(sender) { }

    [HttpPost("{tenantId}/students/{studentId}/requests")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Request([FromRoute] string tenantId, [FromRoute] string studentId, [FromBody] RequestTranscriptCommand body, CancellationToken ct)
        => Ok((await Sender.Send(body with { TenantId = tenantId, StudentId = studentId }, ct)).Value);

    [HttpGet("{tenantId}/requests/{requestId}")]
    [ProducesResponseType(typeof(TranscriptRequestDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromRoute] string tenantId, [FromRoute] string requestId, CancellationToken ct)
        => Ok((await Sender.Send(new GetTranscriptRequestQuery(tenantId, requestId), ct)).Value);
}
