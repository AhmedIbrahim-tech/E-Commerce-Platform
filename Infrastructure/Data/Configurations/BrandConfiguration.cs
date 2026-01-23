namespace Infrastructure.Data.Configurations;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(b => b.Name)
            .ConfigureString(100, isRequired: true);

        builder.Property(b => b.Description)
            .ConfigureString(300, isRequired: false);

        builder.Property(b => b.ImageUrl)
            .ConfigureString(500, isRequired: false);

        builder.Property(b => b.IsActive)
            .ConfigureBoolean(defaultValue: true, isRequired: true);

        builder.Property(b => b.CreatedTime)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        builder.Property(b => b.CreatedBy)
            .ConfigureGuid(isRequired: true);

        builder.Property(b => b.ModifiedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(b => b.ModifiedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(b => b.IsDeleted)
            .ConfigureBoolean(isRequired: true);

        builder.Property(b => b.DeletedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(b => b.DeletedBy)
            .ConfigureGuid(isRequired: false);

        builder.HasIndex(b => b.Name)
            .HasDatabaseName("ix_brands_name");
    }
}
