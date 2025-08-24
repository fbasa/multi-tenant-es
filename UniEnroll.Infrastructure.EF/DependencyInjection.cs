using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniEnroll.Application.Abstractions;
using UniEnroll.Infrastructure.EF.Enrollment;
using UniEnroll.Infrastructure.EF.Persistence;
using UniEnroll.Infrastructure.EF.Repositories;
using UniEnroll.Infrastructure.EF.Security;

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
        });

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<IPermissionService, EfPermissionService>();
        services.AddScoped(typeof(IQueryRepository<>), typeof(EfQueryRepository<>));
        services.AddScoped<IEnrollmentCommandRepository, SqlEnrollmentCommandRepository>();

        return services;
    }
}
