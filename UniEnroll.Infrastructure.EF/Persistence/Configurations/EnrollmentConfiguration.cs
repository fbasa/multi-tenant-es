using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations;

internal sealed class EnrollmentConfiguration : IEntityTypeConfiguration<Domain.Enrollment.Enrollment>
{
    public void Configure(EntityTypeBuilder<Domain.Enrollment.Enrollment> b)
    {
        b.ToTable("Enrollments");
        b.HasKey(x => x.Id);
        b.Property(x => x.TenantId).HasMaxLength(64).IsRequired();
        b.Property(x => x.Status).HasConversion<string>().HasMaxLength(16).IsRequired();
        b.Property(x => x.RowVersion).IsRowVersion().IsConcurrencyToken();
        b.HasIndex(x => new { x.TenantId, x.StudentId, x.SectionId }).IsUnique();
    }
}
