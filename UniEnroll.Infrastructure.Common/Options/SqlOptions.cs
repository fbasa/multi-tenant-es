
namespace UniEnroll.Infrastructure.Common.Options;

public sealed class SqlOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public int CommandTimeoutSeconds { get; set; } = 60;
}
