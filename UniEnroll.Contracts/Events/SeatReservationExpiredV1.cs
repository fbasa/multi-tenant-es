
using System;

namespace UniEnroll.Contracts.Events;

public sealed record SeatReservationExpiredV1(string ReservationId, DateTimeOffset ExpiredAt);
