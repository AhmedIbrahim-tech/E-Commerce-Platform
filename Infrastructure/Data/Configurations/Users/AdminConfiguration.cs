using Domain.Entities.Users;

namespace Infrastructure.Data.Configurations.Users;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(a => a.FullName)
            .ConfigureString(200, isRequired: false);

        builder.Property(a => a.Gender)
            .ConfigureEnum(isRequired: false);

        builder.Property(a => a.Address)
            .ConfigureString(250, isRequired: false);

        builder.Property(a => a.PhoneNumber)
            .ConfigureString(20, isRequired: false);

        builder.Property(a => a.SecondPhoneNumber)
            .ConfigureString(20, isRequired: false);

        builder.Property(a => a.AppUserId)
            .ConfigureGuid(isRequired: true);

        builder.Property(a => a.CreatedTime)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        builder.Property(a => a.CreatedBy)
            .ConfigureGuid(isRequired: true);

        builder.Property(a => a.ModifiedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(a => a.ModifiedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(a => a.IsDeleted)
            .ConfigureBoolean(defaultValue: false, isRequired: true);

        builder.Property(a => a.DeletedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(a => a.DeletedBy)
            .ConfigureGuid(isRequired: false);

        builder.HasIndex(a => a.AppUserId)
            .HasDatabaseName("ix_admins_app_user_id")
            .IsUnique();

        builder.HasIndex(a => a.FullName)
            .HasDatabaseName("ix_admins_full_name");

        builder.HasIndex(a => a.CreatedTime)
            .HasDatabaseName("ix_admins_created_time");
    }
}
