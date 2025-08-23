
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace UniEnroll.Application.Common.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        var response = await next();
        sw.Stop();
        // minimal no-op logging (hook a logger in a real app)
        //var corr = LoggingSafeLogEnricher.GetCorrelationId();
        //System.Diagnostics.Debug.WriteLine($"[{corr}] {typeof(TRequest).Name} handled in {sw.ElapsedMilliseconds}ms");    //TODO:
        return response;
    }
}
