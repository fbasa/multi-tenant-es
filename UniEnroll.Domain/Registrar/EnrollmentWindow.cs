
using System;

namespace UniEnroll.Domain.Registrar;

public sealed class EnrollmentWindow
{
    public string TermId { get; }
    public DateTimeOffset OpensAt { get; }
    public DateTimeOffset ClosesAt { get; }
    public EnrollmentWindow(string termId, DateTimeOffset opensAt, DateTimeOffset closesAt)
    { TermId = termId; OpensAt = opensAt; ClosesAt = closesAt; }
}
