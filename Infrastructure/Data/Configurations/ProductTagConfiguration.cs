namespace Infrastructure.Data.Configurations;

public class ProductTagConfiguration : IEntityTypeConfiguration<ProductTag>
{
    public void Configure(EntityTypeBuilder<ProductTag> builder)
    {
        builder.HasKey(pt => new { pt.ProductId, pt.TagId });

        builder.Property(pt => pt.ProductId)
            .ConfigureGuid(isRequired: true);

        builder.Property(pt => pt.TagId)
            .ConfigureGuid(isRequired: true);

        builder.HasOne(pt => pt.Product)
            .WithMany(p => p.ProductTags)
            .HasForeignKey(pt => pt.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pt => pt.Tag)
            .WithMany(t => t.ProductTags)
            .HasForeignKey(pt => pt.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(pt => pt.ProductId)
            .HasDatabaseName("ix_product_tags_product_id");

        builder.HasIndex(pt => pt.TagId)
            .HasDatabaseName("ix_product_tags_tag_id");
    }
}

