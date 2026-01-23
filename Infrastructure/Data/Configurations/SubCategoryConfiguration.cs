namespace Infrastructure.Data.Configurations;

public class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
{
    public void Configure(EntityTypeBuilder<SubCategory> builder)
    {
        builder.HasKey(sc => sc.Id);

        builder.Property(sc => sc.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(sc => sc.Name)
            .ConfigureString(100, isRequired: true);

        builder.Property(sc => sc.Description)
            .ConfigureString(300, isRequired: false);

        builder.Property(sc => sc.ImageUrl)
            .ConfigureString(500, isRequired: false);

        builder.Property(sc => sc.Code)
            .ConfigureString(50, isRequired: false);

        builder.Property(sc => sc.IsActive)
            .ConfigureBoolean(defaultValue: true, isRequired: true);

        builder.Property(sc => sc.CreatedTime)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        builder.Property(sc => sc.CreatedBy)
            .ConfigureGuid(isRequired: true);

        builder.Property(sc => sc.ModifiedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(sc => sc.ModifiedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(sc => sc.IsDeleted)
            .ConfigureBoolean(isRequired: true);

        builder.Property(sc => sc.DeletedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(sc => sc.DeletedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(sc => sc.CategoryId)
            .ConfigureGuid(isRequired: true);

        builder.HasOne(sc => sc.Category)
            .WithMany()
            .HasForeignKey(sc => sc.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(sc => sc.Name)
            .HasDatabaseName("ix_sub_categories_name");

        builder.HasIndex(sc => sc.CategoryId)
            .HasDatabaseName("ix_sub_categories_category_id");
    }
}
