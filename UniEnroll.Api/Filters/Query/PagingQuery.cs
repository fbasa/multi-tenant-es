using System.ComponentModel.DataAnnotations;

namespace UniEnroll.Api.Filters.Query;

/// <summary>Offset pagination for list endpoints.</summary>
public sealed class PagingQuery
{
    private const int Max = 200;
    [Range(1, int.MaxValue)] public int Page { get; init; } = 1;
    [Range(1, Max)] public int Size { get; init; } = 50;
}
