
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UniEnroll.Domain.Registrar;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Registrar;

public sealed class EnrollmentWindowConfig : IEntityTypeConfiguration<EnrollmentWindow>
{
    public void Configure(EntityTypeBuilder<EnrollmentWindow> b)
    {
        b.ToTable("EnrollmentWindows");
        b.HasKey(x => x.TermId);
    }
}
