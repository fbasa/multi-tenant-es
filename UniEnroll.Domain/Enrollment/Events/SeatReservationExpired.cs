
using UniEnroll.Domain.Abstractions;

namespace UniEnroll.Domain.Enrollment.Events;

public sealed class SeatReservationExpired : DomainEvent
{
    public string ReservationId { get; }
    public SeatReservationExpired(string reservationId) { ReservationId = reservationId; }
}
