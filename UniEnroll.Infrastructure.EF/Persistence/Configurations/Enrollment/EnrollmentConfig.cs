
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Enrollment;

public sealed class EnrollmentConfig : IEntityTypeConfiguration<Domain.Enrollment.Enrollment>
{
    public void Configure(EntityTypeBuilder<Domain.Enrollment.Enrollment> b)
    {
        b.ToTable("Enrollments");
        b.HasKey(x => x.Id);
        b.Property(x => x.RowVersion).IsRowVersion().IsConcurrencyToken();
        b.Property(x => x.TenantId).HasMaxLength(64);
        b.HasIndex(x => new { x.StudentId, x.SectionId }).IsUnique();
    }
}
