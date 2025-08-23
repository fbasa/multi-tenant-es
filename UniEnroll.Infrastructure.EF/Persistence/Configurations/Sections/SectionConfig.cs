
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UniEnroll.Domain.Sections;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Sections;

public sealed class SectionConfig : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> b)
    {
        b.ToTable("Sections");
        b.HasKey(x => x.Id);
        b.Property(x => x.RowVersion).IsRowVersion().IsConcurrencyToken();
        b.Property(x => x.TenantId).HasMaxLength(64).IsRequired();
        b.HasIndex(x => new { x.TermId, x.CourseId });
    }
}
