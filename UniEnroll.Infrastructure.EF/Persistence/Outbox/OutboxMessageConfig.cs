
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UniEnroll.Infrastructure.EF.Persistence.Outbox;

public sealed class OutboxMessageConfig : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> b)
    {
        b.ToTable("OutboxMessages");
        b.HasKey(x => x.Id);
        b.Property(x => x.Type).HasMaxLength(256);
        b.HasIndex(x => x.Processed).HasDatabaseName("IX_Outbox_Processed");
    }
}
