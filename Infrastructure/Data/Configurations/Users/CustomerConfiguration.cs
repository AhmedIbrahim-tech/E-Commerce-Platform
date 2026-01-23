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

        // Foreign key relationships
        // builder.HasMany(c => c.Orders)
        //     .WithOne(o => o.Customer)
        //     .HasForeignKey(o => o.CustomerId)
        //     .OnDelete(DeleteBehavior.Restrict);

        // builder.HasMany(c => c.ShippingAddresses)
        //     .WithOne(sa => sa.Customer)
        //     .HasForeignKey(sa => sa.CustomerId)
        //     .OnDelete(DeleteBehavior.Cascade);

        // builder.HasMany(c => c.Reviews)
        //     .WithOne(r => r.Customer)
        //     .HasForeignKey(r => r.CustomerId)
        //     .OnDelete(DeleteBehavior.Cascade);

        // Index for AppUserId lookup
        builder.HasIndex(c => c.AppUserId)
            .HasDatabaseName("ix_customers_app_user_id")
            .IsUnique();
    }
}
