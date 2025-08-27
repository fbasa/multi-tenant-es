
using System;

namespace UniEnroll.Infrastructure.Common.Abstractions;

public interface IDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
    DateTimeOffset NowLocal { get; }
    DateOnly TodayLocal { get; }
}
