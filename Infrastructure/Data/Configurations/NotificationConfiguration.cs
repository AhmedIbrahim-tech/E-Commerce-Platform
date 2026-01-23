using Domain.Entities.Notifications;
using Infrastructure.Data.Configurations;

namespace Infrastructure.Data.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type)
            .ConfigureString(150, isRequired: true);

        builder.Property(x => x.RecipientRole)
            .ConfigureEnum(isRequired: true)
            .HasMaxLength(50);

        builder.Property(x => x.RecipientId)
            .ConfigureGuid(isRequired: true);

        builder.Property(x => x.Data)
            .HasColumnType("nvarchar(max)")
            .IsRequired();

        builder.Property(x => x.IsRead)
            .ConfigureBoolean(defaultValue: false, isRequired: true);

        builder.Property(x => x.CreatedAt)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        builder.HasIndex(x => new { x.RecipientRole, x.RecipientId, x.IsRead });
        builder.HasIndex(x => new { x.RecipientRole, x.RecipientId, x.CreatedAt });
    }
}

