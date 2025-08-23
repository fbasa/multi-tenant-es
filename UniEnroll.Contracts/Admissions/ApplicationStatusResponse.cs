
using System;

namespace UniEnroll.Contracts.Admissions;

public sealed record ApplicationStatusResponse(string ApplicationId, string Status, DateTimeOffset UpdatedAt);
