
using System.Diagnostics;
using MediatR;

namespace UniEnroll.Application.Common.Behaviors;

public sealed class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        var resp = await next();
        sw.Stop();
        Debug.WriteLine($"[Perf] {typeof(TRequest).Name} took {sw.ElapsedMilliseconds}ms");
        return resp;
    }
}
