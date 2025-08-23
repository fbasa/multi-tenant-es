using System.ComponentModel.DataAnnotations;

namespace UniEnroll.Api.Filters.Query;

public sealed class KeysetQuery
{
    [MaxLength(2048)] public string? Next { get; init; }
    [MaxLength(2048)] public string? Prev { get; init; }
    [Range(1, 200)] public int PageSize { get; init; } = 50;
}
