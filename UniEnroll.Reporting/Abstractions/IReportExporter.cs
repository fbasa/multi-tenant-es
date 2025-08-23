
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UniEnroll.Reporting.Abstractions;

public interface IReportExporter
{
    /// <summary>Short name like "csv" or "xlsx".</summary>
    string Name { get; }

    /// <summary>HTTP content type for download.</summary>
    string ContentType { get; }

    /// <summary>Exports arbitrary rows into a binary payload ready for download.</summary>
    Task<byte[]> ExportAsync<T>(IEnumerable<T> rows, CancellationToken ct = default);
}
