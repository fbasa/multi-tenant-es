
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace UniEnroll.Infrastructure.EF.Persistence.Interceptors;

public sealed class DispatchDomainEventsInterceptor : SaveChangesInterceptor
{
    // Placeholder: domain events would be gathered from aggregates and published via IEventBus.
}
