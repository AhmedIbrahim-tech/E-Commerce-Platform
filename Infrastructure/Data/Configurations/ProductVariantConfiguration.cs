namespace Infrastructure.Data.Configurations;

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.HasKey(pv => pv.Id);

        builder.Property(pv => pv.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(pv => pv.VariantAttribute)
            .ConfigureString(100, isRequired: true);

        builder.Property(pv => pv.VariantValue)
            .ConfigureString(100, isRequired: true);

        builder.Property(pv => pv.SKU)
            .ConfigureString(100, isRequired: true);

        builder.Property(pv => pv.Quantity)
            .ConfigureInteger(isRequired: true);

        builder.Property(pv => pv.Price)
            .ConfigureDecimal(precision: 18, scale: 2, isRequired: true);

        builder.Property(pv => pv.ImageURL)
            .ConfigureString(500, isRequired: false);

        builder.Property(pv => pv.IsActive)
            .ConfigureBoolean(isRequired: true);

        builder.Property(pv => pv.CreatedTime)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        builder.Property(pv => pv.CreatedBy)
            .ConfigureGuid(isRequired: true);

        builder.Property(pv => pv.ModifiedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(pv => pv.ModifiedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(pv => pv.IsDeleted)
            .ConfigureBoolean(isRequired: true);

        builder.Property(pv => pv.DeletedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(pv => pv.DeletedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(pv => pv.ProductId)
            .ConfigureGuid(isRequired: true);

        builder.HasIndex(pv => pv.ProductId)
            .HasDatabaseName("ix_product_variants_product_id");

        builder.HasIndex(pv => pv.SKU)
            .IsUnique()
            .HasDatabaseName("ix_product_variants_sku");
    }
}
