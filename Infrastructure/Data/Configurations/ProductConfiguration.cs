namespace Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever()
            .ConfigureGuid("id", isRequired: true);

        builder.Property(p => p.Name)
            .ConfigureString("name", 100, isRequired: true);

        builder.Property(p => p.Description)
            .ConfigureString("description", 300, isRequired: false);

        builder.Property(p => p.Price)
            .ConfigureDecimal("price", precision: 18, scale: 2, isRequired: true);

        builder.Property(p => p.StockQuantity)
            .ConfigureInteger("stock_quantity", isRequired: true);

        builder.Property(p => p.ImageURL)
            .ConfigureString("image_url", 500, isRequired: false);

        builder.Property(p => p.CreatedAt)
            .ConfigureTimestamp("created_at", hasDefaultValue: true, isRequired: true);

        builder.Property(p => p.CategoryId)
            .ConfigureGuid("category_id", isRequired: true);

        // Foreign key relationship
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes for performance
        builder.HasIndex(p => p.Name)
            .HasDatabaseName("ix_products_name");

        builder.HasIndex(p => p.CategoryId)
            .HasDatabaseName("ix_products_category_id");

        builder.HasIndex(p => p.CreatedAt)
            .HasDatabaseName("ix_products_created_at");
    }
}
