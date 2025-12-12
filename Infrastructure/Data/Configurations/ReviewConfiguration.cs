namespace Infrastructure.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("reviews");

        // Composite primary key
        builder.HasKey(r => new { r.CustomerId, r.ProductId });

        builder.Property(r => r.CustomerId)
            .ConfigureGuid("customer_id", isRequired: true);

        builder.Property(r => r.ProductId)
            .ConfigureGuid("product_id", isRequired: true);

        builder.Property(r => r.Rating)
            .ConfigureEnum("rating", isRequired: true);

        builder.Property(r => r.Comment)
            .ConfigureString("comment", 400, isRequired: false);

        builder.Property(r => r.CreatedAt)
            .ConfigureTimestamp("created_at", hasDefaultValue: true, isRequired: true);

        // Foreign key relationships
        builder.HasOne(r => r.Customer)
            .WithMany(c => c.Reviews)
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Product)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for performance
        builder.HasIndex(r => r.ProductId)
            .HasDatabaseName("ix_reviews_product_id");

        builder.HasIndex(r => r.CustomerId)
            .HasDatabaseName("ix_reviews_customer_id");

        builder.HasIndex(r => r.Rating)
            .HasDatabaseName("ix_reviews_rating");
    }
}
