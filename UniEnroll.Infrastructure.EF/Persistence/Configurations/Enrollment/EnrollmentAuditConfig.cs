
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UniEnroll.Domain.Enrollment;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Enrollment;

public sealed class EnrollmentAuditConfig : IEntityTypeConfiguration<EnrollmentAudit>
{
    public void Configure(EntityTypeBuilder<EnrollmentAudit> b)
    {
        b.ToTable("EnrollmentAudits");
        b.HasKey(x => x.Id);
        b.Property(x => x.Action).HasMaxLength(32);
    }
}
