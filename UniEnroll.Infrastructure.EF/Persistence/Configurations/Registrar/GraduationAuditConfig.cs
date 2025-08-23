
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UniEnroll.Domain.Registrar;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Registrar;

public sealed class GraduationAuditConfig : IEntityTypeConfiguration<GraduationAudit>
{
    public void Configure(EntityTypeBuilder<GraduationAudit> b)
    {
        b.ToTable("GraduationAudits");
        b.HasKey(x => x.Id);
    }
}
