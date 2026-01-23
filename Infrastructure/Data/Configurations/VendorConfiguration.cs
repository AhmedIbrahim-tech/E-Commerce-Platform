using Domain.Entities.Users;

namespace Infrastructure.Data.Configurations;

public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(v => v.FullName)
            .ConfigureString(200, isRequired: false);

        builder.Property(v => v.Gender)
            .ConfigureEnum(isRequired: false);

        builder.Property(v => v.StoreName)
            .ConfigureString(200, isRequired: true);

        builder.Property(v => v.CommissionRate)
            .ConfigureDecimal(precision: 5, scale: 2, isRequired: true);

        builder.Property(v => v.PhoneNumber)
            .ConfigureString(20, isRequired: false);

        builder.Property(v => v.SecondPhoneNumber)
            .ConfigureString(20, isRequired: false);

        builder.Property(v => v.AppUserId)
            .ConfigureGuid(isRequired: true);

        builder.Property(v => v.CreatedTime)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        builder.Property(v => v.CreatedBy)
            .ConfigureGuid(isRequired: true);

        builder.Property(v => v.ModifiedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(v => v.ModifiedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(v => v.IsDeleted)
            .ConfigureBoolean(defaultValue: false, isRequired: true);

        builder.Property(v => v.DeletedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(v => v.DeletedBy)
            .ConfigureGuid(isRequired: false);

        // Index for AppUserId lookup
        builder.HasIndex(v => v.AppUserId)
            .HasDatabaseName("ix_vendors_app_user_id")
            .IsUnique();
    }
}
