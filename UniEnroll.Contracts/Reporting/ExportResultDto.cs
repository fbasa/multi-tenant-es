
namespace UniEnroll.Contracts.Reporting;

public sealed record ExportResultDto(string JobId, string Status, string? DownloadUrl);
