using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniEnroll.Domain.Enrollment;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations;

internal sealed class EnrollmentAuditConfiguration : IEntityTypeConfiguration<EnrollmentAudit>
{
    public void Configure(EntityTypeBuilder<EnrollmentAudit> b)
    {
        b.ToTable("EnrollmentAudits");
        b.HasKey(x => x.Id);
        b.Property(x => x.Action).HasMaxLength(32).IsRequired();
        b.Property(x => x.ActorUserId).HasMaxLength(64).IsRequired();
        b.Property(x => x.Reason).HasMaxLength(512);
        b.HasIndex(x => x.EnrollmentId);
    }
}
