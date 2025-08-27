
using Microsoft.EntityFrameworkCore;
using UniEnroll.Infrastructure.EF.Query.Views;

namespace UniEnroll.Infrastructure.EF.Persistence;

public sealed class ReadDbContext : DbContext
{
    public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options) { }

    public DbSet<StudentDashboardView> StudentDashboards => Set<StudentDashboardView>();
    public DbSet<CourseSearchView> CourseSearch => Set<CourseSearchView>();
    public DbSet<DegreeProgressView> DegreeProgress => Set<DegreeProgressView>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentDashboardView>().HasNoKey().ToView("vw_StudentDashboard");
        modelBuilder.Entity<CourseSearchView>().HasNoKey().ToView("vw_CourseSearch");
        modelBuilder.Entity<DegreeProgressView>().HasNoKey().ToView("vw_DegreeProgress");
        base.OnModelCreating(modelBuilder);
    }
}
