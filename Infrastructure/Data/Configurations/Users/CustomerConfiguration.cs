using Domain.Entities.Users;

namespace Infrastructure.Data.Configurations.Users;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(c => c.FullName)
            .ConfigureString(200, isRequired: false);

        builder.Property(c => c.Gender)
            .ConfigureEnum(isRequired: false);

        builder.Property(c => c.PhoneNumber)
            .ConfigureString(20, isRequired: false);

        builder.Property(c => c.SecondPhoneNumber)
            .ConfigureString(20, isRequired: false);

        builder.Property(c => c.AppUserId)
            .ConfigureGuid(isRequired: true);

        builder.Property(c => c.CreatedTime)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        builder.Property(c => c.CreatedBy)
            .ConfigureGuid(isRequired: true);

        builder.Property(c => c.ModifiedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(c => c.ModifiedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(c => c.IsDeleted)
            .ConfigureBoolean(defaultValue: false, isRequired: true);

        builder.Property(c => c.DeletedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(c => c.DeletedBy)
            .ConfigureGuid(isRequired: false);

        builder.HasIndex(c => c.AppUserId)
            .HasDatabaseName("ix_customers_app_user_id")
            .IsUnique();

        builder.HasIndex(c => c.FullName)
            .HasDatabaseName("ix_customers_full_name");

        builder.HasIndex(c => c.CreatedTime)
            .HasDatabaseName("ix_customers_created_time");
    }
}
