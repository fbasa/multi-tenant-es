using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UniEnroll.Domain.Sections;
using UniEnroll.Domain.Sections.ValueObjects;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations;

internal sealed class SectionConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> b)
    {
        b.ToTable("Sections");
        b.HasKey(x => x.Id);
        b.Property(x => x.TenantId).HasMaxLength(64).IsRequired();

        //var capConv = new ValueConverter<Capacity, string>(
        //    v => $"{v.Total}|{v.Waitlist}",
        //    v => {
        //        var p = (v ?? "0|0").Split('|');
        //        var tot = int.TryParse(p[0], out var t) ? t : 0;
        //        var wl = int.TryParse(p.Length > 1 ? p[1] : "0", out var w) ? w : 0;
        //        return new Capacity(tot, wl);
        //    });
        b.Property(x => x.Capacity)
            //.HasConversion(capConv)
            .HasMaxLength(32).IsRequired();

        //var roomConv = new ValueConverter<Room?, string?>(
        //    v => v?.Code,
        //    v => string.IsNullOrWhiteSpace(v) ? (Room?)null : new Room(v!));
        b.Property(x => x.Room)
            //.HasConversion(roomConv)
            .HasMaxLength(32);
        b.Property(x => x.MeetingDays)
            //.HasConversion(
            //v => string.Join(',', v ?? Array.Empty<DayOfWeek>()),
            //v => string.IsNullOrWhiteSpace(v) 
            //    ? Array.Empty<DayOfWeek>() 
            //    : Array.ConvertAll(v.Split(','), s => Enum.Parse<DayOfWeek>(s))
            //)
            .HasMaxLength(64);

        b.Property(x => x.StartTime).IsRequired();
        b.Property(x => x.EndTime).IsRequired();
        b.Property(x => x.SeatsTaken).IsRequired();
        b.Property(x => x.RowVersion).IsRowVersion().IsConcurrencyToken();
        b.HasIndex(x => new { x.TenantId, x.CourseId, x.TermId });
    }
}
