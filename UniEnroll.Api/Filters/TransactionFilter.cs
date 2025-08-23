
using System.Transactions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UniEnroll.Api.Filters;

/// <summary>Wraps action in an ambient transaction with ReadCommitted snapshot.</summary>
public sealed class TransactionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted
        }, TransactionScopeAsyncFlowOption.Enabled);
        context.HttpContext.Items["__txscope"] = scope;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.HttpContext.Items.TryGetValue("__txscope", out var s) && s is TransactionScope scope)
        {
            if (context.Exception is null) scope.Complete();
            scope.Dispose();
        }
    }
}
