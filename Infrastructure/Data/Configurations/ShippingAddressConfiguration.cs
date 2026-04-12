using Domain.Entities.Shipping;

namespace Infrastructure.Data.Configurations;

public class ShippingAddressConfiguration : IEntityTypeConfiguration<ShippingAddress>
{
    public void Configure(EntityTypeBuilder<ShippingAddress> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(s => s.FullName)
            .ConfigureString(150, isRequired: true);

        builder.Property(s => s.Street)
            .ConfigureString(150, isRequired: true);

        builder.Property(s => s.City)
            .ConfigureString(150, isRequired: true);

        builder.Property(s => s.State)
            .ConfigureString(150, isRequired: true);

        builder.Property(s => s.CustomerId)
            .ConfigureGuid(isRequired: true);

        // Index for customer lookup
        builder.HasIndex(s => s.CustomerId)
            .HasDatabaseName("ix_shipping_addresses_customer_id");
    }
}
