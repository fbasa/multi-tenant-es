
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UniEnroll.BackgroundWorker.Scheduling;

namespace UniEnroll.BackgroundWorker.Jobs;

public sealed class PaymentStatusSyncJob : BackgroundService
{
    private readonly ILogger<PaymentStatusSyncJob> _logger;
    private readonly IConfiguration _config;
    private readonly string _jobKey = "jobs.paymentstatussync";

    public PaymentStatusSyncJob(ILogger<PaymentStatusSyncJob> logger, IConfiguration config)
    { _logger = logger; _config = config; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PaymentStatusSyncJob started with cron: {Cron}", CronExpressions.PaymentStatusSync);

        while (!stoppingToken.IsCancellationRequested)
        {
            var delay = GetInterval();
            try
            {
                await RunOnceAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PaymentStatusSyncJob execution failed");
            }
            await Task.Delay(delay, stoppingToken);
        }
    }

    private TimeSpan GetInterval()
    {
        var def = _config.GetValue<int?>("Jobs:Defaults:IntervalSeconds") ?? 300;
        var val = _config.GetValue<int?>("Jobs:PaymentStatusSync:IntervalSeconds") ?? def;
        return TimeSpan.FromSeconds(Math.Max(5, val));
    }

    private Task RunOnceAsync(CancellationToken ct)
    {
        _logger.LogInformation("PaymentStatusSyncJob tick at {Now:o}", DateTimeOffset.UtcNow);
        // TODO: implement actual logic
        return Task.CompletedTask;
    }
}
