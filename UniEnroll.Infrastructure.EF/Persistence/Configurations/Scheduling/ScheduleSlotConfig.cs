
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UniEnroll.Domain.Scheduling;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Scheduling;

public sealed class ScheduleSlotConfig : IEntityTypeConfiguration<ScheduleSlot>
{
    public void Configure(EntityTypeBuilder<ScheduleSlot> b)
    {
        b.ToTable("ScheduleSlots");
        b.HasKey(x => x.SectionId);
    }
}
