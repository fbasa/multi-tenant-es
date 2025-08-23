using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace UniEnroll.Infrastructure.Common.Tenancy;

public interface ITenantResolver
{
    Task<string?> ResolveAsync(HttpContext httpContext);
}
