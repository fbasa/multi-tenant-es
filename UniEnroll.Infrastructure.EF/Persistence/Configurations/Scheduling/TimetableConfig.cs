
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UniEnroll.Domain.Scheduling;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Scheduling;

public sealed class TimetableConfig : IEntityTypeConfiguration<Timetable>
{
    public void Configure(EntityTypeBuilder<Timetable> b)
    {
        b.ToTable("Timetables");
        b.HasKey("StudentId");
    }
}
