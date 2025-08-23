using System;

namespace UniEnroll.Contracts.Registrar;

public sealed record HoldDto(
    string Id,
    string StudentId,
    string HoldType,
    string Status,
    DateTimeOffset? ExpiresAt);
