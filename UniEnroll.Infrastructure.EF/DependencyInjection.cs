using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniEnroll.Infrastructure.Common.Abstractions;
using UniEnroll.Infrastructure.EF.Persistence;
using UniEnroll.Infrastructure.EF.Persistence.Idempotency;
using UniEnroll.Infrastructure.EF.Persistence.Interceptors;
using UniEnroll.Infrastructure.EF.Persistence.Outbox;
using UniEnroll.Infrastructure.EF.Repositories;
using UniEnroll.Infrastructure.EF.Repositories.Contracts;

namespace UniEnroll.Infrastructure.EF;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureEf(this IServiceCollection services, IConfiguration config)
    {
        var cs = config.GetConnectionString("SqlServer") ?? throw new InvalidOperationException("Missing SQL connection string");

        // EF interceptors
        services.AddScoped<AuditableSaveChangesInterceptor>();
        services.AddScoped<DispatchDomainEventsInterceptor>();
        services.AddScoped<TenantWriteInterceptor>();

        services.AddDbContextPool<UniEnrollDbContext>((sp, options) =>
        {
            options.UseSqlServer(cs, sql =>
            {
                sql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
                sql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });

            /*
            Note: If you wrap several operations in your own transaction, 
            run them under the execution strategy so the whole unit can retry:

            var strategy = db.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await db.Database.BeginTransactionAsync(ct);
                // �multiple ops�
                await db.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);
            });

            
            */
            options.AddInterceptors(
               sp.GetRequiredService<AuditableSaveChangesInterceptor>(),
               sp.GetRequiredService<DispatchDomainEventsInterceptor>(),
               sp.GetRequiredService<TenantWriteInterceptor>()
           );

        });

        services.AddDbContext<ReadDbContext>((sp,options) =>
        {
            options.UseSqlServer(cs);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));

        // Outbox
        services.AddScoped<OutboxProcessor>();
        
        // Idempotency store SQL
        services.AddScoped<IIdempotencyStore, IdempotencyStore>();

        return services;
    }
}
