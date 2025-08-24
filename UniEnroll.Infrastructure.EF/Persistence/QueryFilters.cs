using Microsoft.EntityFrameworkCore;
using UniEnroll.Domain.Courses;
using UniEnroll.Domain.Identity;
using UniEnroll.Domain.Sections;
using UniEnroll.Domain.Students;

namespace UniEnroll.Infrastructure.EF.Persistence;

/// <summary>Applies per-entity query filters bound to the current TenantId.</summary>
public static class QueryFilters
{
    public static void Apply(ModelBuilder modelBuilder)
    {
        // Note: EF can't dynamically iterate nav at runtime for filters, so explicitly register.
        modelBuilder.Entity<Student>().HasQueryFilter(e => TenantIdHolder.CurrentTenantId == null || e.TenantId == TenantIdHolder.CurrentTenantId);
        modelBuilder.Entity<Course>().HasQueryFilter(e => TenantIdHolder.CurrentTenantId == null || e.TenantId == TenantIdHolder.CurrentTenantId);
        modelBuilder.Entity<Section>().HasQueryFilter(e => TenantIdHolder.CurrentTenantId == null || e.TenantId == TenantIdHolder.CurrentTenantId);
        modelBuilder.Entity<Domain.Enrollment.Enrollment>().HasQueryFilter(e => TenantIdHolder.CurrentTenantId == null || e.TenantId == TenantIdHolder.CurrentTenantId);
        modelBuilder.Entity<Domain.Admissions.Application>().HasQueryFilter(e => TenantIdHolder.CurrentTenantId == null || e.TenantId == TenantIdHolder.CurrentTenantId);
        modelBuilder.Entity<User>().HasQueryFilter(e => TenantIdHolder.CurrentTenantId == null || e.TenantId == TenantIdHolder.CurrentTenantId);
    }

    /// <summary>
    /// Holder read by the filter expressions. Set via API middleware (see EfTenantSetter).
    /// </summary>
    public static class TenantIdHolder
    {
        public static string? CurrentTenantId { get; set; }
    }
}
