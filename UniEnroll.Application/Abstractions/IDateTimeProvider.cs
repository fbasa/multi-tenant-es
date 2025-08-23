
using System;

namespace UniEnroll.Application.Abstractions;

public interface IDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
    DateTimeOffset NowLocal { get; }
    DateOnly TodayLocal { get; }
}
