
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace UniEnroll.Infrastructure.EF.Persistence.Interceptors;

public sealed class SeatReservationExpiryInterceptor : SaveChangesInterceptor
{
    // Placeholder: prune expired SeatReservations opportunistically.
}
