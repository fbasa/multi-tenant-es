using System;
using UniEnroll.Infrastructure.Common.Abstractions;

namespace UniEnroll.Infrastructure.Common.Time;

public sealed class SystemClock : IDateTimeProvider
{
    private static readonly TimeZoneInfo _ph = TimeZoneInfo.FindSystemTimeZoneById(
#if WINDOWS
        "Singapore Standard Time"
#else
        "Asia/Manila"
#endif
    );

    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    public DateTimeOffset NowLocal => TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, _ph);
    public DateOnly TodayLocal => DateOnly.FromDateTime(NowLocal.DateTime);
}
