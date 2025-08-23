
using System;

namespace UniEnroll.Infrastructure.Common.Localization;

public sealed class ManilaTimeZoneProvider
{
    public TimeZoneInfo TimeZone { get; } = TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila");
}
