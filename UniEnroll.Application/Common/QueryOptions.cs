
namespace UniEnroll.Application.Common;

public sealed class QueryOptions
{
    public int Page { get; init; } = 1;
    public int Size { get; init; } = 50;
    public string? Sort { get; init; }
}
