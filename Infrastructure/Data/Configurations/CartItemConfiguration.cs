using Domain.Entities.Cart;

namespace Infrastructure.Data.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(ci => new { ci.CartId, ci.ProductId });

        builder.Property(ci => ci.CartId)
            .ConfigureGuid(isRequired: true);

        builder.Property(ci => ci.ProductId)
            .ConfigureGuid(isRequired: true);

        builder.Property(ci => ci.Price)
            .ConfigureDecimal(precision: 18, scale: 2, isRequired: false);

        builder.Property(ci => ci.Quantity)
            .ConfigureInteger(isRequired: false);

        builder.Property(ci => ci.SubAmount)
            .ConfigureDecimal(precision: 18, scale: 2, isRequired: false);

        builder.Property(ci => ci.CreatedTime)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

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

