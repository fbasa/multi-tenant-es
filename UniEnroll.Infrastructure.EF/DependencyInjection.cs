using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniEnroll.Infrastructure.EF.Persistence;
using UniEnroll.Infrastructure.EF.Repositories;
using UniEnroll.Infrastructure.EF.Repositories.Contracts;

namespace UniEnroll.Infrastructure.EF;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureEf(this IServiceCollection services, IConfiguration config)
    {
        var cs = config.GetConnectionString("SqlServer") ?? "Server=(localdb)\\mssqllocaldb;Database=UniEnroll;Trusted_Connection=True;MultipleActiveResultSets=true";

        services.AddDbContextPool<UniEnrollDbContext>(options =>
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
                // …multiple ops…
                await db.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);
            });

            
            */
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        services.AddScoped<IPermissionRepository, PermissionRepository>();

        services.AddScoped<IInstructorCommandRepository, InstructorCommandRepository>();
        services.AddScoped<IRegistrarCommandRepository, RegistrarCommandRepository>();

        services.AddScoped<IPaymentCommandRepository, PaymentCommandRepository>();
        services.AddScoped<IPaymentQueryRepository, PaymentQueryRepository>();
        services.AddScoped<ISchedulingRepository, SchedulingRepository>();

        return services;
    }
}
