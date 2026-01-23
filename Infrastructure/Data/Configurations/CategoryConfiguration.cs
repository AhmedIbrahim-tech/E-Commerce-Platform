using Domain.Entities.Catalog;

namespace Infrastructure.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(c => c.Name)
            .ConfigureString(100, isRequired: true);

        builder.Property(c => c.Description)
            .ConfigureString(300, isRequired: false);

        // Index for name lookup
        builder.HasIndex(c => c.Name)
            .HasDatabaseName("ix_categories_name");
    }
}
