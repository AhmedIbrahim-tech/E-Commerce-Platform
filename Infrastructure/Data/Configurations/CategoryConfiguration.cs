namespace Infrastructure.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever()
            .ConfigureGuid("id", isRequired: true);

        builder.Property(c => c.Name)
            .ConfigureString("name", 100, isRequired: true);

        builder.Property(c => c.Description)
            .ConfigureString("description", 300, isRequired: false);

        // Index for name lookup
        builder.HasIndex(c => c.Name)
            .HasDatabaseName("ix_categories_name");
    }
}
