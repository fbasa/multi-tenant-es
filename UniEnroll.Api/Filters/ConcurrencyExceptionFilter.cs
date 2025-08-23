
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace UniEnroll.Api.Filters;

/// <summary>Maps EF concurrency exceptions to 409 problem details.</summary>
public sealed class ConcurrencyExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is DbUpdateConcurrencyException)
        {
            context.Result = new ObjectResult(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Concurrency conflict",
                Detail = "The resource was modified by another request. Please retry."
            }) { StatusCode = StatusCodes.Status409Conflict };
            context.ExceptionHandled = true;
        }
    }
}
