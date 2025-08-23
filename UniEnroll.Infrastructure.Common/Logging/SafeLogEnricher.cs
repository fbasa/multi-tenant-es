using System.Threading;

namespace UniEnroll.Infrastructure.Common.Logging;

/// <summary>
/// Holds correlation id per async flow without requiring a specific logging provider.
/// </summary>
public static class SafeLogEnricher
{
    private static readonly AsyncLocal<string?> _corr = new();

    public static void SetCorrelationId(string? id) => _corr.Value = id;
    public static string? GetCorrelationId() => _corr.Value;
}
