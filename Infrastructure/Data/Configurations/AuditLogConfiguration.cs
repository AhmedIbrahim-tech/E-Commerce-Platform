using Domain.Entities.AuditLogs;

namespace Infrastructure.Data.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(a => a.EventType)
            .ConfigureString(100, isRequired: true);

        builder.Property(a => a.EventName)
            .ConfigureString(200, isRequired: true);

        builder.Property(a => a.Description)
            .ConfigureString(1000, isRequired: false);

        builder.Property(a => a.UserId)
            .ConfigureGuid(isRequired: false);

        builder.Property(a => a.UserEmail)
            .ConfigureString(256, isRequired: false);

        builder.Property(a => a.AdditionalData)
            .ConfigureString(2000, isRequired: false);

        builder.Property(a => a.CreatedTime)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        // Indexes for performance
        builder.HasIndex(a => a.EventType)
            .HasDatabaseName("ix_audit_logs_event_type");

        builder.HasIndex(a => a.UserId)
            .HasDatabaseName("ix_audit_logs_user_id");

        builder.HasIndex(a => a.CreatedTime)
            .HasDatabaseName("ix_audit_logs_created_time");
    }
}
