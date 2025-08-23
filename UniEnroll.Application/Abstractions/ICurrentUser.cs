
namespace UniEnroll.Application.Abstractions;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    string? UserId { get; }
    string? Email { get; }
    string[] Roles { get; }
    string? TenantId { get; }
}
