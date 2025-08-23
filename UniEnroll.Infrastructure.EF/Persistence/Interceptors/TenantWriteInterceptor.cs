
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace UniEnroll.Infrastructure.EF.Persistence.Interceptors;

public sealed class TenantWriteInterceptor : SaveChangesInterceptor
{
    // Placeholder: set TenantId on new entities if not already set (via ITenantContext).
}
