
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniEnroll.BackgroundWorker.Jobs;

namespace UniEnroll.BackgroundWorker;

public static class DependencyInjection
{
    public static IServiceCollection AddBackgroundWorkerServices(this IServiceCollection services, IConfiguration config)
    {
        // Register hosted jobs
        services.AddHostedService<ProcessOutboxJob>();
        services.AddHostedService<PaymentStatusSyncJob>();
        services.AddHostedService<RebuildGpaAggregatesJob>();
        services.AddHostedService<OptimizeSchedulingJob>();
        services.AddHostedService<ReportRefreshJob>();
        services.AddHostedService<GraduationAuditBatchJob>();
        services.AddHostedService<SeatReservationExpiryJob>();
        services.AddHostedService<WaitlistPromotionJob>();
        services.AddHostedService<EnrollmentWindowNotifierJob>();
        services.AddHostedService<PaymentReminderJob>();
        services.AddHostedService<GradePostedNotifierJob>();

        return services;
    }
}
