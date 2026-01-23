using Domain.Enums;

namespace Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(p => p.Name)
            .ConfigureString(200, isRequired: true);

        builder.Property(p => p.Slug)
            .ConfigureString(250, isRequired: true);

        builder.Property(p => p.SKU)
            .ConfigureString(100, isRequired: true);

        builder.Property(p => p.Description)
            .ConfigureString(2000, isRequired: false);

        builder.Property(p => p.ShortDescription)
            .ConfigureString(500, isRequired: false);

        builder.Property(p => p.Price)
            .ConfigureDecimal(precision: 18, scale: 2, isRequired: true);

        builder.Property(p => p.StockQuantity)
            .ConfigureInteger(isRequired: true);

        builder.Property(p => p.QuantityAlert)
            .ConfigureInteger(isRequired: true);

        builder.Property(p => p.Barcode)
            .ConfigureString(100, isRequired: false);

        builder.Property(p => p.BarcodeSymbology)
            .ConfigureString(50, isRequired: false);

        builder.Property(p => p.ManufacturedDate)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(p => p.ExpiryDate)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(p => p.Manufacturer)
            .ConfigureString(200, isRequired: false);

        builder.Property(p => p.IsActive)
            .ConfigureBoolean(isRequired: true);

        builder.Property(p => p.PublishStatus)
            .HasConversion<int>()
            .HasDefaultValue(ProductPublishStatus.Published)
            .IsRequired();

        builder.Property(p => p.Visibility)
            .HasConversion<int>()
            .HasDefaultValue(ProductVisibility.Public)
            .IsRequired();

        builder.Property(p => p.PublishDate)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(p => p.ProductType)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(p => p.SellingType)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(p => p.TaxType)
            .HasConversion<int?>();

        builder.Property(p => p.TaxRate)
            .ConfigureDecimal(precision: 5, scale: 2, isRequired: false);

        builder.Property(p => p.DiscountType)
            .HasConversion<int?>();

        builder.Property(p => p.DiscountValue)
            .ConfigureDecimal(precision: 18, scale: 2, isRequired: false);

        builder.Property(p => p.CreatedTime)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        builder.Property(p => p.CreatedBy)
            .ConfigureGuid(isRequired: true);

        builder.Property(p => p.ModifiedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(p => p.ModifiedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(p => p.IsDeleted)
            .ConfigureBoolean(isRequired: true);

        builder.Property(p => p.DeletedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(p => p.DeletedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(p => p.CategoryId)
            .ConfigureGuid(isRequired: true);

        builder.Property(p => p.SubCategoryId)
            .ConfigureGuid(isRequired: false);

        builder.Property(p => p.BrandId)
            .ConfigureGuid(isRequired: false);

        builder.Property(p => p.UnitOfMeasureId)
            .ConfigureGuid(isRequired: false);

        builder.Property(p => p.WarrantyId)
            .ConfigureGuid(isRequired: false);

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.SubCategory)
            .WithMany(sc => sc.Products)
            .HasForeignKey(p => p.SubCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Brand)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.UnitOfMeasure)
            .WithMany(u => u.Products)
            .HasForeignKey(p => p.UnitOfMeasureId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Warranty)
            .WithMany(w => w.Products)
            .HasForeignKey(p => p.WarrantyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.ProductImages)
            .WithOne(pi => pi.Product)
            .HasForeignKey(pi => pi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.ProductVariants)
            .WithOne(pv => pv.Product)
            .HasForeignKey(pv => pv.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.Name)
            .HasDatabaseName("ix_products_name");

        builder.HasIndex(p => p.Slug)
            .IsUnique()
            .HasDatabaseName("ix_products_slug");

        builder.HasIndex(p => p.SKU)
            .IsUnique()
            .HasDatabaseName("ix_products_sku");

        builder.HasIndex(p => p.CategoryId)
            .HasDatabaseName("ix_products_category_id");

        builder.HasIndex(p => p.SubCategoryId)
            .HasDatabaseName("ix_products_sub_category_id");

        builder.HasIndex(p => p.BrandId)
            .HasDatabaseName("ix_products_brand_id");

        builder.HasIndex(p => p.CreatedTime)
            .HasDatabaseName("ix_products_created_time");
    }
}
