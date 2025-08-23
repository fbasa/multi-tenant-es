
namespace UniEnroll.Application.Common.Pagination;

public sealed class KeysetPageRequest
{
    public string? Next { get; init; }
    public string? Prev { get; init; }
    public int PageSize { get; init; } = 50;
}
