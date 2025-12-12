namespace Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever()
            .ConfigureGuid("id", isRequired: true);

        builder.Property(c => c.FullName)
            .ConfigureString("full_name", 200, isRequired: false);

        builder.Property(c => c.Gender)
            .ConfigureEnum("gender", isRequired: false);

        builder.Property(c => c.AppUserId)
            .ConfigureGuid("app_user_id", isRequired: true);

        // Foreign key relationships
        builder.HasMany(c => c.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.ShippingAddresses)
            .WithOne(sa => sa.Customer)
            .HasForeignKey(sa => sa.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Reviews)
            .WithOne(r => r.Customer)
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Index for AppUserId lookup
        builder.HasIndex(c => c.AppUserId)
            .HasDatabaseName("ix_customers_app_user_id")
            .IsUnique();
    }
}
