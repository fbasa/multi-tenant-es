using System;

namespace UniEnroll.Contracts.Scheduling;

public sealed record TimetableDto(
    string OwnerId,
    string TermId,
    TimetableEntryDto[] Entries);
