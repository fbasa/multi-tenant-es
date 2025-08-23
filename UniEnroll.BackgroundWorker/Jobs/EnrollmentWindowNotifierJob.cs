
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UniEnroll.BackgroundWorker.Scheduling;

namespace UniEnroll.BackgroundWorker.Jobs;

public sealed class EnrollmentWindowNotifierJob : BackgroundService
{
    private readonly ILogger<EnrollmentWindowNotifierJob> _logger;
    private readonly IConfiguration _config;
    private readonly string _jobKey = "jobs.enrollmentwindownotifier";

    public EnrollmentWindowNotifierJob(ILogger<EnrollmentWindowNotifierJob> logger, IConfiguration config)
    { _logger = logger; _config = config; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("EnrollmentWindowNotifierJob started with cron: {Cron}", CronExpressions.EnrollmentWindowNotifier);

        while (!stoppingToken.IsCancellationRequested)
        {
            var delay = GetInterval();
            try
            {
                await RunOnceAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EnrollmentWindowNotifierJob execution failed");
            }
            await Task.Delay(delay, stoppingToken);
        }
    }

    private TimeSpan GetInterval()
    {
        var def = _config.GetValue<int?>("Jobs:Defaults:IntervalSeconds") ?? 300;
        var val = _config.GetValue<int?>("Jobs:EnrollmentWindowNotifier:IntervalSeconds") ?? def;
        return TimeSpan.FromSeconds(Math.Max(5, val));
    }

    private Task RunOnceAsync(CancellationToken ct)
    {
        _logger.LogInformation("EnrollmentWindowNotifierJob tick at {Now:o}", DateTimeOffset.UtcNow);
        // TODO: implement actual logic
        return Task.CompletedTask;
    }
}
