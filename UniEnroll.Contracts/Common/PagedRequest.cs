
namespace UniEnroll.Contracts.Common;

public sealed record PagedRequest(int Page = 1, int Size = 50, string? Sort = null);
