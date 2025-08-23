
namespace UniEnroll.Application.Abstractions;

public interface ITenantContext
{
    string? TenantId { get; set; }
}
