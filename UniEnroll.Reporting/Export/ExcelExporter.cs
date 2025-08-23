
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UniEnroll.Reporting.Abstractions;

namespace UniEnroll.Reporting.Export;

/// <summary>
/// Minimal "Excel" exporter that returns CSV bytes but advertises Excel content-type; Excel opens CSV seamlessly.
/// Swap with a true OpenXML implementation when desired.
/// </summary>
public sealed class ExcelExporter : IReportExporter
{
    private readonly CsvExporter _csv = new();
    public string Name => "xlsx";
    public string ContentType => "application/vnd.ms-excel";

    public Task<byte[]> ExportAsync<T>(IEnumerable<T> rows, CancellationToken ct = default)
        => _csv.ExportAsync(rows, ct);
}
