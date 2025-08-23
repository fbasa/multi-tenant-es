
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UniEnroll.Api.Filters;

public sealed class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var pd = new ValidationProblemDetails(context.ModelState) { Status = StatusCodes.Status400BadRequest, Title = "Validation failed" };
            pd.Extensions["correlationId"] = context.HttpContext.Items.TryGetValue("X-Correlation-Id", out var c) ? c : null;
            context.Result = new ObjectResult(pd) { StatusCode = StatusCodes.Status400BadRequest };
        }
    }
    public void OnActionExecuted(ActionExecutedContext context) { }
}
