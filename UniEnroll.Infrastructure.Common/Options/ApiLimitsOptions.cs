namespace UniEnroll.Infrastructure.Common.Options;

public sealed class ApiLimitsOptions
{
    public int MaxPageSize { get; set; } = 200;
    public int MaxKeysetPageSize { get; set; } = 200;
}
