namespace Infrastructure.Data.Configurations;

public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.ToTable("deliveries");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .ValueGeneratedNever()
            .ConfigureGuid("id", isRequired: true);

        builder.Property(d => d.DeliveryMethod)
            .ConfigureEnum("delivery_method", isRequired: true);

        builder.Property(d => d.Description)
            .ConfigureString("description", 300, isRequired: false);

        builder.Property(d => d.DeliveryTime)
            .ConfigureTimestamp("delivery_time", hasDefaultValue: false, isRequired: false);

        builder.Property(d => d.Cost)
            .ConfigureDecimal("cost", precision: 18, scale: 2, isRequired: false);

        builder.Property(d => d.Status)
            .ConfigureEnum("status", isRequired: false);

        // Index for delivery method lookup
        builder.HasIndex(d => d.DeliveryMethod)
            .HasDatabaseName("ix_deliveries_delivery_method");

        builder.HasIndex(d => d.Status)
            .HasDatabaseName("ix_deliveries_status");
    }
}
