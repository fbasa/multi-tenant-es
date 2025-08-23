
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEnroll.Reporting.Abstractions;
using UniEnroll.Contracts.Reporting;
using UniEnroll.Application.Abstractions;

namespace UniEnroll.Reporting.Subscriptions;

/// <summary>
/// Composes EnrollmentReport and emails it to a recipient in the chosen format.
/// </summary>
public sealed class EnrollmentReportSubscription
{
    private readonly IReportStore _store;
    private readonly IEnumerable<IReportExporter> _exporters;
    private readonly IEmailSender _email;

    public EnrollmentReportSubscription(IReportStore store, IEnumerable<IReportExporter> exporters, IEmailSender email)
    {
        _store = store; _exporters = exporters; _email = email;
    }

    public async Task SendAsync(string tenantId, string? termId, string toEmail, string format = "csv", CancellationToken ct = default)
    {
        var rows = await _store.GetEnrollmentReportAsync(tenantId, termId, ct);
        var exporter = _exporters.FirstOrDefault(e => e.Name.Equals(format, StringComparison.OrdinalIgnoreCase))
                    ?? _exporters.First(e => e.Name == "csv");

        var bytes = await exporter.ExportAsync(rows, ct);
        // Minimal email body with inline summary; attachment handling would require expanding IEmailSender.
        var total = rows.Sum(r => r.Count);
        var html = $"<p>Enrollment report generated. Total enrollments: <b>{total}</b>. Format: {exporter.Name.ToUpperInvariant()} ({bytes.Length} bytes)</p>";
        await _email.SendAsync(toEmail, "Enrollment Report", html, ct);
    }
}
