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

        builder.Property(s => s.FirstName)
            .ConfigureString(100, isRequired: true);

        builder.Property(s => s.LastName)
            .ConfigureString(100, isRequired: true);

        builder.Property(s => s.Street)
            .ConfigureString(150, isRequired: true);

        builder.Property(s => s.City)
            .ConfigureString(150, isRequired: true);

        builder.Property(s => s.State)
            .ConfigureString(150, isRequired: true);

        builder.Property(s => s.CustomerId)
            .ConfigureGuid(isRequired: true);

        // Foreign key relationship
        //builder.HasOne(s => s.Customer)
        //    .WithMany(c => c.ShippingAddresses)
        //    .HasForeignKey(s => s.CustomerId)
        //    .OnDelete(DeleteBehavior.Cascade);

        // Index for customer lookup
        builder.HasIndex(s => s.CustomerId)
            .HasDatabaseName("ix_shipping_addresses_customer_id");
    }
}
