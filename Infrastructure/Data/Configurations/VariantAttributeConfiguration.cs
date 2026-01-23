namespace Infrastructure.Data.Configurations;

public class VariantAttributeConfiguration : IEntityTypeConfiguration<VariantAttribute>
{
    public void Configure(EntityTypeBuilder<VariantAttribute> builder)
    {
        builder.HasKey(va => va.Id);

        builder.Property(va => va.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(va => va.Name)
            .ConfigureString(100, isRequired: true);

        builder.Property(va => va.Description)
            .ConfigureString(300, isRequired: false);

        builder.Property(va => va.IsActive)
            .ConfigureBoolean(defaultValue: true, isRequired: true);

        builder.Property(va => va.CreatedTime)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        builder.Property(va => va.CreatedBy)
            .ConfigureGuid(isRequired: true);

        builder.Property(va => va.ModifiedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(va => va.ModifiedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(va => va.IsDeleted)
            .ConfigureBoolean(isRequired: true);

        builder.Property(va => va.DeletedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(va => va.DeletedBy)
            .ConfigureGuid(isRequired: false);

        builder.HasMany(va => va.Values)
            .WithOne(vav => vav.VariantAttribute)
            .HasForeignKey(vav => vav.VariantAttributeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(va => va.Name)
            .HasDatabaseName("ix_variant_attributes_name");
    }
}

public class VariantAttributeValueConfiguration : IEntityTypeConfiguration<VariantAttributeValue>
{
    public void Configure(EntityTypeBuilder<VariantAttributeValue> builder)
    {
        builder.HasKey(vav => vav.Id);

        builder.Property(vav => vav.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(vav => vav.Value)
            .ConfigureString(100, isRequired: true);

        builder.Property(vav => vav.DisplayName)
            .ConfigureString(100, isRequired: false);

        builder.Property(vav => vav.IsActive)
            .ConfigureBoolean(defaultValue: true, isRequired: true);

        builder.Property(vav => vav.CreatedTime)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        builder.Property(vav => vav.CreatedBy)
            .ConfigureGuid(isRequired: true);

        builder.Property(vav => vav.ModifiedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(vav => vav.ModifiedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(vav => vav.IsDeleted)
            .ConfigureBoolean(isRequired: true);

        builder.Property(vav => vav.DeletedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(vav => vav.DeletedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(vav => vav.VariantAttributeId)
            .ConfigureGuid(isRequired: true);

        builder.HasIndex(vav => vav.VariantAttributeId)
            .HasDatabaseName("ix_variant_attribute_values_variant_attribute_id");
    }
}
