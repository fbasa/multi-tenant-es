
namespace UniEnroll.BackgroundWorker.Scheduling;

public static class CronExpressions
{
    public const string ProcessOutbox = "*/30 * * * * *";              // every 30s
    public const string PaymentStatusSync = "0 */2 * * * *";            // every 2m
    public const string RebuildGpaAggregates = "0 */30 * * * *";        // every 30m
    public const string OptimizeScheduling = "0 */15 * * * *";          // every 15m
    public const string ReportRefresh = "0 */30 * * * *";               // every 30m
    public const string GraduationAuditBatch = "0 0 * * * *";           // hourly
    public const string SeatReservationExpiry = "*/60 * * * * *";       // every 60s
    public const string WaitlistPromotion = "*/60 * * * * *";           // every 60s
    public const string EnrollmentWindowNotifier = "0 */5 * * * *";     // every 5m
    public const string PaymentReminder = "0 0 9 * * *";                // daily 9am
    public const string GradePostedNotifier = "0 */2 * * * *";          // every 2m
}
