
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UniEnroll.Domain.Registrar;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Registrar;

public sealed class PrerequisiteRuleConfig : IEntityTypeConfiguration<PrerequisiteRule>
{
    public void Configure(EntityTypeBuilder<PrerequisiteRule> b)
    {
        b.ToTable("PrerequisiteRules");
        b.HasKey(x => x.CourseId);
    }
}
