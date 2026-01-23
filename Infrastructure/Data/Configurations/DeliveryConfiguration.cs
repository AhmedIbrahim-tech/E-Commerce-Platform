using Domain.Entities.Orders;

namespace Infrastructure.Data.Configurations;

public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(d => d.DeliveryMethod)
            .ConfigureEnum(isRequired: true);

        builder.Property(d => d.Description)
            .ConfigureString(300, isRequired: false);

        builder.Property(d => d.DeliveryTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(d => d.Cost)
            .ConfigureDecimal(precision: 18, scale: 2, isRequired: false);

        builder.Property(d => d.Status)
            .ConfigureEnum(isRequired: false);

        // Index for delivery method lookup
        builder.HasIndex(d => d.DeliveryMethod)
            .HasDatabaseName("ix_deliveries_delivery_method");

        builder.HasIndex(d => d.Status)
            .HasDatabaseName("ix_deliveries_status");
    }
}
