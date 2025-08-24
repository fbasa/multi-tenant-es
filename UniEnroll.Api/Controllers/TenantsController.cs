
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Api.Auth;

namespace UniEnroll.Api.Controllers;

[Authorize(Policy = Policies.Registrar.ManageTenants)]
public sealed class TenantsController : BaseApiController
{
    public TenantsController(MediatR.ISender sender) : base(sender) { }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult List() => Ok(new[] { new { Id = "default", Name = "Default" } });
}
