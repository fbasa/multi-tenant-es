namespace UniEnroll.Infrastructure.Common.Abstractions;

public interface ITenantContext
{
    string? TenantId { get; set; }
}
