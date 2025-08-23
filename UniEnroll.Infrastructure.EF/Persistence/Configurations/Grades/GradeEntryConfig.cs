
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UniEnroll.Domain.Grades;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Grades;

public sealed class GradeEntryConfig : IEntityTypeConfiguration<GradeEntry>
{
    public void Configure(EntityTypeBuilder<GradeEntry> b)
    {
        b.ToTable("Grades");
        b.HasKey(x => x.Id);
        b.HasIndex(x => x.EnrollmentId);
    }
}
