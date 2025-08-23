
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UniEnroll.Domain.Registrar;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Registrar;

public sealed class AcademicCalendarConfig : IEntityTypeConfiguration<AcademicCalendar>
{
    public void Configure(EntityTypeBuilder<AcademicCalendar> b)
    {
        b.ToTable("AcademicCalendars");
        b.HasKey("TermId");
    }
}
