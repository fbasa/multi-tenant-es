
using System;

namespace UniEnroll.Contracts.Events;

public sealed record TermPublishedV1(string TermId, string YearTermCode, DateTimeOffset OccurredAt);
