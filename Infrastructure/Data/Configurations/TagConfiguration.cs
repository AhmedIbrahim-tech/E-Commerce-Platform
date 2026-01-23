namespace Infrastructure.Data.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(t => t.Name)
            .ConfigureString(100, isRequired: true);

        builder.Property(t => t.IsActive)
            .ConfigureBoolean(defaultValue: true, isRequired: true);

        builder.Property(t => t.CreatedTime)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        builder.Property(t => t.CreatedBy)
            .ConfigureGuid(isRequired: true);

        builder.Property(t => t.ModifiedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(t => t.ModifiedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(t => t.IsDeleted)
            .ConfigureBoolean(isRequired: true);

        builder.Property(t => t.DeletedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(t => t.DeletedBy)
            .ConfigureGuid(isRequired: false);

        builder.HasIndex(t => t.Name)
            .IsUnique()
            .HasDatabaseName("ix_tags_name");
    }
}

