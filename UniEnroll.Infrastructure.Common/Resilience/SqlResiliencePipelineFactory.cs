
using System;
using System.Data.Common;
using Polly;
using Polly.Retry;

namespace UniEnroll.Infrastructure.Common.Resilience;

public static class SqlResiliencePipelineFactory
{
    public static AsyncRetryPolicy CreateTransientRetryPolicy()
        => Policy
            .Handle<DbException>()
            .Or<TimeoutException>()
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt)));
}
