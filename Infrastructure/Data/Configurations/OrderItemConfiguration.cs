namespace Infrastructure.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");

        // Composite primary key
        builder.HasKey(oi => new { oi.ProductId, oi.OrderId });

        builder.Property(oi => oi.ProductId)
            .ConfigureGuid("product_id", isRequired: true);

        builder.Property(oi => oi.OrderId)
            .ConfigureGuid("order_id", isRequired: true);

        builder.Property(oi => oi.Quantity)
            .ConfigureInteger("quantity", isRequired: true);

        builder.Property(oi => oi.UnitPrice)
            .ConfigureDecimal("unit_price", precision: 18, scale: 2, isRequired: true);

        builder.Property(oi => oi.SubAmount)
            .ConfigureDecimal("sub_amount", precision: 18, scale: 2, isRequired: false);

        // Foreign key relationships
        builder.HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for performance
        builder.HasIndex(oi => oi.OrderId)
            .HasDatabaseName("ix_order_items_order_id");

        builder.HasIndex(oi => oi.ProductId)
            .HasDatabaseName("ix_order_items_product_id");
    }
}
