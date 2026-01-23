using Domain.Entities.Cart;

namespace Infrastructure.Data.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(c => c.CustomerId);

        builder.Property(c => c.CustomerId)
            .ValueGeneratedNever()
            .ConfigureGuid(isRequired: true);

        builder.Property(c => c.CreatedTime)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: false);

        builder.Property(c => c.PaymentToken)
            .ConfigureString(500, isRequired: false);

        builder.Property(c => c.PaymentIntentId)
            .ConfigureString(500, isRequired: false);

        builder.Property(c => c.TotalAmount)
            .ConfigureDecimal(precision: 18, scale: 2, isRequired: false);

        builder.HasMany(c => c.CartItems)
            .WithOne()
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

