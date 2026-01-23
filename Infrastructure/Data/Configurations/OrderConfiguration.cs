namespace Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(o => o.OrderDate)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        builder.Property(o => o.CreatedTime)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        builder.Property(o => o.CreatedBy)
            .ConfigureGuid(isRequired: true);

        builder.Property(o => o.ModifiedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(o => o.ModifiedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(o => o.Status)
            .ConfigureEnum(isRequired: true);

        builder.Property(o => o.TotalAmount)
            .ConfigureDecimal(precision: 18, scale: 2, isRequired: true);

        builder.Property(o => o.CustomerId)
            .ConfigureGuid(isRequired: true);

        builder.Property(o => o.ShippingAddressId)
            .ConfigureGuid(isRequired: false);

        builder.Property(o => o.PaymentId)
            .ConfigureGuid(isRequired: false);

        builder.Property(o => o.DeliveryId)
            .ConfigureGuid(isRequired: false);

        builder.Ignore(o => o.PaymentToken);

        // Foreign key relationships
        builder.HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(o => o.ShippingAddress)
            .WithOne()
            .HasForeignKey<Order>(o => o.ShippingAddressId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(o => o.Payment)
            .WithOne()
            .HasForeignKey<Order>(o => o.PaymentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Delivery)
            .WithOne()
            .HasForeignKey<Order>(o => o.DeliveryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes for performance
        builder.HasIndex(o => o.CustomerId)
            .HasDatabaseName("ix_orders_customer_id");

        builder.HasIndex(o => o.Status)
            .HasDatabaseName("ix_orders_status");

        builder.HasIndex(o => o.OrderDate)
            .HasDatabaseName("ix_orders_order_date");

        builder.HasIndex(o => o.PaymentId)
            .HasDatabaseName("ix_orders_payment_id")
            .IsUnique();

        builder.HasIndex(o => o.DeliveryId)
            .HasDatabaseName("ix_orders_delivery_id")
            .IsUnique();
    }
}
