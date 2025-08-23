
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniEnroll.Reporting.Abstractions;
using UniEnroll.Reporting.Export;
using UniEnroll.Reporting.Stores;
using UniEnroll.Reporting.Refresh;

namespace UniEnroll.Reporting;

public static class DependencyInjection
{
    /// <summary>Registers reporting store, exporters, and refresh service.</summary>
    public static IServiceCollection AddReporting(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IReportStore, SqlReportStore>();

        // register both exporters and let callers pick by Name
        services.AddSingleton<IReportExporter, CsvExporter>();
        services.AddSingleton<IReportExporter, ExcelExporter>();

        // optional background refresher (disabled by default; enable via options if needed)
        services.AddHostedService<ReportRefreshService>();

        return services;
    }
}
