
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UniEnroll.Domain.Students;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Students;

public sealed class NotificationPreferencesConfig : IEntityTypeConfiguration<NotificationPreferences>
{
    public void Configure(EntityTypeBuilder<NotificationPreferences> b)
    {
        b.ToTable("StudentNotificationPrefs");
        b.HasKey("StudentId");
    }
}
