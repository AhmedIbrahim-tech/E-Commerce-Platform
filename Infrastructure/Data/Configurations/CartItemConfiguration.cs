namespace Infrastructure.Data.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("cart_items");

        builder.HasKey(ci => new { ci.CartId, ci.ProductId });

        builder.Property(ci => ci.CartId)
            .ConfigureGuid("cart_id", isRequired: true);

        builder.Property(ci => ci.ProductId)
            .ConfigureGuid("product_id", isRequired: true);

        builder.Property(ci => ci.Price)
            .ConfigureDecimal("price", precision: 18, scale: 2, isRequired: false);

        builder.Property(ci => ci.Quantity)
            .ConfigureInteger("quantity", isRequired: false);

        builder.Property(ci => ci.SubAmount)
            .ConfigureDecimal("sub_amount", precision: 18, scale: 2, isRequired: false);

        builder.Property(ci => ci.CreatedAt)
            .ConfigureTimestamp("created_at", hasDefaultValue: true, isRequired: true);

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(ci => ci.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(ci => ci.CartId)
            .HasDatabaseName("ix_cart_items_cart_id");

        builder.HasIndex(ci => ci.ProductId)
            .HasDatabaseName("ix_cart_items_product_id");
    }
}

