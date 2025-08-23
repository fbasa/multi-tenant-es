
using System;

namespace UniEnroll.Domain.Enrollment;

public sealed class SeatReservation
{
    public string Id { get; }
    public string SectionId { get; }
    public string StudentId { get; }
    public DateTimeOffset ExpiresAt { get; }
    public bool IsExpired => DateTimeOffset.UtcNow >= ExpiresAt;

    public SeatReservation(string id, string sectionId, string studentId, DateTimeOffset expiresAt)
    { Id = id; SectionId = sectionId; StudentId = studentId; ExpiresAt = expiresAt; }
}
