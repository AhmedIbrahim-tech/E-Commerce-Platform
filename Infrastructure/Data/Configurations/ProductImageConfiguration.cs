namespace Infrastructure.Data.Configurations;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.HasKey(pi => pi.Id);

        builder.Property(pi => pi.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(pi => pi.ImageURL)
            .ConfigureString(500, isRequired: true);

        builder.Property(pi => pi.IsPrimary)
            .ConfigureBoolean(isRequired: true);

        builder.Property(pi => pi.DisplayOrder)
            .ConfigureInteger(isRequired: true);

        builder.Property(pi => pi.CreatedTime)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        builder.Property(pi => pi.CreatedBy)
            .ConfigureGuid(isRequired: true);

        builder.Property(pi => pi.ModifiedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(pi => pi.ModifiedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(pi => pi.IsDeleted)
            .ConfigureBoolean(isRequired: true);

        builder.Property(pi => pi.DeletedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(pi => pi.DeletedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(pi => pi.ProductId)
            .ConfigureGuid(isRequired: true);

        builder.HasIndex(pi => pi.ProductId)
            .HasDatabaseName("ix_product_images_product_id");
    }
}
