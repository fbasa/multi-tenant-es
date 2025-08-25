using Microsoft.AspNetCore.Mvc;
using System.Linq;
using UniEnroll.Infrastructure.Common.Tenancy;

namespace UniEnroll.Api.Support;

public static class ProblemFactory
{
    public static IActionResult FromModelState(ActionContext ctx)
    {
        var details = new ValidationProblemDetails(ctx.ModelState)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed"
        };
        details.Extensions["correlationId"] = ctx.HttpContext.Items.TryGetValue(TenantHeaderNames.CorrelationId, out var v) ? v : null;
        return new ObjectResult(details) { StatusCode = StatusCodes.Status400BadRequest };
    }
}
