using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Application.Features.Identity.Commands.AssignRole;
using UniEnroll.Application.Features.Identity.Commands.RegisterUser;
using UniEnroll.Application.Features.Identity.Queries.GetMe;

namespace UniEnroll.Api.Controllers;

public sealed class IdentityController : BaseApiController
{
    public IdentityController(ISender sender) : base(sender) { }

    [AllowAnonymous]
    [HttpPost("{tenantId}/register")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromRoute] string tenantId, [FromBody] RegisterUserCommand body, CancellationToken ct)
        => Ok((await Sender.Send(body with { TenantId = tenantId }, ct)).Value);

    [HttpPost("{tenantId}/assign-role")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AssignRole([FromRoute] string tenantId, [FromBody] AssignRoleCommand body, CancellationToken ct)
        => Ok(await Sender.Send(body with { TenantId = tenantId }, ct));

    [HttpGet("me")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Me(CancellationToken ct) => Ok((await Sender.Send(new GetMeQuery(), ct)).Value);
}
