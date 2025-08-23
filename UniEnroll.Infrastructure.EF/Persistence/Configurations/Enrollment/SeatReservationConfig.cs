
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UniEnroll.Domain.Enrollment;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Enrollment;

public sealed class SeatReservationConfig : IEntityTypeConfiguration<SeatReservation>
{
    public void Configure(EntityTypeBuilder<SeatReservation> b)
    {
        b.ToTable("SeatReservations");
        b.HasKey(x => x.Id);
        b.HasIndex(x => new { x.SectionId, x.StudentId }).IsUnique();
    }
}
