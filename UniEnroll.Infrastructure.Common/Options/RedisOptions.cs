
namespace UniEnroll.Infrastructure.Common.Options;

public sealed class RedisOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string? InstanceName { get; set; }
}
