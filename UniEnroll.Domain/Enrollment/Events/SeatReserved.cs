
using System;
using UniEnroll.Domain.Abstractions;

namespace UniEnroll.Domain.Enrollment.Events;

public sealed class SeatReserved : DomainEvent
{
    public string ReservationId { get; }
    public DateTimeOffset ExpiresAt { get; }
    public SeatReserved(string reservationId, DateTimeOffset expiresAt) { ReservationId = reservationId; ExpiresAt = expiresAt; }
}
