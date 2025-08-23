using Microsoft.EntityFrameworkCore;
using UniEnroll.Domain.Tenancy;
using UniEnroll.Domain.Identity;
using UniEnroll.Domain.Students;
using UniEnroll.Domain.Courses;
using UniEnroll.Domain.Sections;
using UniEnroll.Domain.Enrollment;
using UniEnroll.Domain.Admissions;

namespace UniEnroll.Infrastructure.EF.Persistence;

public sealed class UniEnrollDbContext : DbContext
{
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Section> Sections => Set<Section>();
    public DbSet<Domain.Enrollment.Enrollment> Enrollments => Set<Domain.Enrollment.Enrollment>();
    public DbSet<EnrollmentAudit> EnrollmentAudits => Set<EnrollmentAudit>();
    public DbSet<Domain.Admissions.Application> Applications => Set<Domain.Admissions.Application>();

    public UniEnrollDbContext(DbContextOptions<UniEnrollDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply configurations
        modelBuilder.ApplyConfiguration(new Configurations.TenantConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.UserConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.StudentConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.CourseConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.SectionConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.EnrollmentConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.EnrollmentAuditConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.ApplicationConfiguration());

        // Global query filters for multi-tenancy
        QueryFilters.Apply(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
}
