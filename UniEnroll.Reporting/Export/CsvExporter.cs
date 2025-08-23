
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEnroll.Reporting.Abstractions;

namespace UniEnroll.Reporting.Export;

public sealed class CsvExporter : IReportExporter
{
    public string Name => "csv";
    public string ContentType => "text/csv";

    public Task<byte[]> ExportAsync<T>(IEnumerable<T> rows, CancellationToken ct = default)
    {
        var list = rows?.ToList() ?? new List<T>();
        if (list.Count == 0) return Task.FromResult(Encoding.UTF8.GetBytes(string.Empty));

        var props = typeof(T).GetProperties();
        var sb = new StringBuilder();
        sb.AppendLine(string.Join(",", props.Select(p => Escape(p.Name))));
        foreach (var item in list)
        {
            var values = props.Select(p => Escape(p.GetValue(item)?.ToString() ?? string.Empty));
            sb.AppendLine(string.Join(",", values));
        }
        return Task.FromResult(Encoding.UTF8.GetBytes(sb.ToString()));
    }

    private static string Escape(string s)
    {
        if (s.Contains(',') || s.Contains('"') || s.Contains('\n'))
            return s;   //TODO
            //return $""{s.Replace("""","""")}"";
        return s;
    }
}
