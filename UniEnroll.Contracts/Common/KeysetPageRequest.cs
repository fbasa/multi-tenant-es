
namespace UniEnroll.Contracts.Common;

public sealed record KeysetPageRequest(string? Next = null, string? Prev = null, int PageSize = 50);
