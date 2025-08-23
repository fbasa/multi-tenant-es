
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UniEnroll.Reporting.Abstractions;

namespace UniEnroll.Reporting.Refresh;

/// <summary>
/// Background refresh to warm caches or precompute heavy reports (no-ops by default).
/// </summary>
public sealed class ReportRefreshService : BackgroundService
{
    private readonly ILogger<ReportRefreshService> _logger;
    private readonly IReportStore _store;

    public ReportRefreshService(ILogger<ReportRefreshService> logger, IReportStore store)
    {
        _logger = logger; _store = store;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ReportRefreshService started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // placeholder: warm minimal dataset for current year per tenant (tenant-less in this skeleton)
                await _store.GetRetentionCohortReportAsync(tenantId: "default", cohortYear: DateTimeOffset.UtcNow.Year, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Report refresh tick failed");
            }
            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
}
