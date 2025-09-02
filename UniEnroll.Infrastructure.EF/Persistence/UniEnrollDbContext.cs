using Microsoft.EntityFrameworkCore;
using UniEnroll.Domain.Tenancy;
using UniEnroll.Domain.Identity;
using UniEnroll.Domain.Students;
using UniEnroll.Domain.Courses;
using UniEnroll.Domain.Sections;
using UniEnroll.Domain.Enrollment;
using UniEnroll.Domain.Admissions;
using UniEnroll.Infrastructure.EF.Persistence.Configurations;

namespace UniEnroll.Infrastructure.EF.Persistence;

public sealed class UniEnrollDbContext : DbContext
{
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Section> Sections => Set<Section>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<EnrollmentAudit> EnrollmentAudits => Set<EnrollmentAudit>();
    public DbSet<Application> Applications => Set<Application>();

    public UniEnrollDbContext(DbContextOptions<UniEnrollDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply configurations
        modelBuilder.ApplyConfiguration(new TenantConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new StudentConfiguration());
        modelBuilder.ApplyConfiguration(new CourseConfiguration());
        modelBuilder.ApplyConfiguration(new SectionConfiguration());
        modelBuilder.ApplyConfiguration(new EnrollmentConfiguration());
        modelBuilder.ApplyConfiguration(new EnrollmentAuditConfiguration());
        modelBuilder.ApplyConfiguration(new ApplicationConfiguration());


        // Global query filters for multi-tenancy
        QueryFilters.Apply(modelBuilder);
        base.OnModelCreating(modelBuilder);
        SoftDeleteConvention.ApplyToModel(modelBuilder);
    }
}
