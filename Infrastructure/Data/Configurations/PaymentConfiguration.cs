namespace Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever()
            .ConfigureGuid("id", isRequired: true);

        builder.Property(p => p.OrderId)
            .ConfigureGuid("order_id", isRequired: true);

        builder.Property(p => p.TransactionId)
            .ConfigureString("transaction_id", 200, isRequired: false);

        builder.Property(p => p.PaymentDate)
            .ConfigureTimestamp("payment_date", hasDefaultValue: false, isRequired: false);

        builder.Property(p => p.PaymentMethod)
            .ConfigureEnum("payment_method", isRequired: true);

        builder.Property(p => p.TotalAmount)
            .ConfigureDecimal("total_amount", precision: 18, scale: 2, isRequired: false);

        builder.Property(p => p.Status)
            .ConfigureEnum("status", isRequired: true);

        // Indexes for performance
        builder.HasIndex(p => p.OrderId)
            .HasDatabaseName("ix_payments_order_id")
            .IsUnique();

        builder.HasIndex(p => p.TransactionId)
            .HasDatabaseName("ix_payments_transaction_id")
            .IsUnique();

        builder.HasIndex(p => p.Status)
            .HasDatabaseName("ix_payments_status");

        builder.HasIndex(p => p.PaymentMethod)
            .HasDatabaseName("ix_payments_payment_method");
    }
}
