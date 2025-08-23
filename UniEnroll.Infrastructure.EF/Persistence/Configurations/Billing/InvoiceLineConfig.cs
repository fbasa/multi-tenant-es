
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UniEnroll.Domain.Billing;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Billing;

public sealed class InvoiceLineConfig : IEntityTypeConfiguration<InvoiceLine>
{
    public void Configure(EntityTypeBuilder<InvoiceLine> b)
    {
        b.ToTable("InvoiceLines");
        b.HasKey(x => x.Id);
        b.Property(x => x.Description).HasMaxLength(256);
    }
}
