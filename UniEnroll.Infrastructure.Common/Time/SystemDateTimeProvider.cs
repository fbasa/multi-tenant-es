
using System;
using UniEnroll.Infrastructure.Common.Abstractions;

namespace UniEnroll.Infrastructure.Common.Time;

public sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

    public DateTimeOffset NowLocal => throw new NotImplementedException();  //TODO:

    public DateOnly TodayLocal => throw new NotImplementedException();

    public DateTime LocalNow(string timeZoneId = "Asia/Manila")
        => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(timeZoneId));
}
