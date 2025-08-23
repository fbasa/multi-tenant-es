
using Microsoft.EntityFrameworkCore;
using UniEnroll.Domain.Sections;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Sections;

public static class SectionIndexes
{
    public static void ConfigureIndexes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Section>()
            .HasIndex(s => new { s.TermId, s.InstructorId });
    }
}
