namespace Infrastructure.Data.Configurations;

public class ShippingAddressConfiguration : IEntityTypeConfiguration<ShippingAddress>
{
    public void Configure(EntityTypeBuilder<ShippingAddress> builder)
    {
        builder.ToTable("shipping_addresses");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .ValueGeneratedNever()
            .ConfigureGuid("id", isRequired: true);

        builder.Property(s => s.FirstName)
            .ConfigureString("first_name", 100, isRequired: true);

        builder.Property(s => s.LastName)
            .ConfigureString("last_name", 100, isRequired: true);

        builder.Property(s => s.Street)
            .ConfigureString("street", 150, isRequired: true);

        builder.Property(s => s.City)
            .ConfigureString("city", 150, isRequired: true);

        builder.Property(s => s.State)
            .ConfigureString("state", 150, isRequired: true);

        builder.Property(s => s.CustomerId)
            .ConfigureGuid("customer_id", isRequired: true);

        // Foreign key relationship
        builder.HasOne(s => s.Customer)
            .WithMany(c => c.ShippingAddresses)
            .HasForeignKey(s => s.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Index for customer lookup
        builder.HasIndex(s => s.CustomerId)
            .HasDatabaseName("ix_shipping_addresses_customer_id");
    }
}
