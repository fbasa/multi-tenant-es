using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace UniEnroll.Infrastructure.Common.Abstractions;

public interface ITenantResolver
{
    Task<string?> ResolveAsync(HttpContext httpContext);
}
