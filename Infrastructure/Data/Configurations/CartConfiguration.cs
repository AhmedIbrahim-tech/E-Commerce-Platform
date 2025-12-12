namespace Infrastructure.Data.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("carts");

        builder.HasKey(c => c.CustomerId);

        builder.Property(c => c.CustomerId)
            .ValueGeneratedNever()
            .ConfigureGuid("customer_id", isRequired: true);

        builder.Property(c => c.CreatedAt)
            .ConfigureTimestamp("created_at", hasDefaultValue: true, isRequired: false);

        builder.Property(c => c.PaymentToken)
            .ConfigureString("payment_token", 500, isRequired: false);

        builder.Property(c => c.PaymentIntentId)
            .ConfigureString("payment_intent_id", 500, isRequired: false);

        builder.Property(c => c.TotalAmount)
            .ConfigureDecimal("total_amount", precision: 18, scale: 2, isRequired: false);

        builder.HasMany(c => c.CartItems)
            .WithOne()
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

