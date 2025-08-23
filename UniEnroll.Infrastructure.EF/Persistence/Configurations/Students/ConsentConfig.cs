
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UniEnroll.Domain.Students;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Students;

public sealed class ConsentConfig : IEntityTypeConfiguration<Consent>
{
    public void Configure(EntityTypeBuilder<Consent> b)
    {
        b.ToTable("StudentConsents");
        b.HasKey(x => new { x.StudentId, x.Type });
    }
}
