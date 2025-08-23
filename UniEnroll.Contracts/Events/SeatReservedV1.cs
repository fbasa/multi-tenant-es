
using System;

namespace UniEnroll.Contracts.Events;

public sealed record SeatReservedV1(string ReservationId, string SectionId, string StudentId, DateTimeOffset ExpiresAt);
