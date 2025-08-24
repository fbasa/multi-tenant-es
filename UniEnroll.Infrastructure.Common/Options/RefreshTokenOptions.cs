
namespace UniEnroll.Infrastructure.Common.Options;

public sealed class RefreshTokenOptions
{
    public int Days { get; set; } = 14;                 // lifetime
    public string CookieName { get; set; } = "rt";      // cookie name
    public bool UseCookies { get; set; } = true;        // store in HttpOnly cookie
}
