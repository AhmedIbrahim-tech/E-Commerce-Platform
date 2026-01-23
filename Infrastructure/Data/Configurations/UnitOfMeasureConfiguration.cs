namespace Infrastructure.Data.Configurations;

public class UnitOfMeasureConfiguration : IEntityTypeConfiguration<UnitOfMeasure>
{
    public void Configure(EntityTypeBuilder<UnitOfMeasure> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(u => u.Name)
            .ConfigureString(100, isRequired: true);

        builder.Property(u => u.ShortName)
            .ConfigureString(20, isRequired: true);

        builder.Property(u => u.Description)
            .ConfigureString(300, isRequired: false);

        builder.Property(u => u.IsActive)
            .ConfigureBoolean(defaultValue: true, isRequired: true);

        builder.Property(u => u.CreatedTime)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        builder.Property(u => u.CreatedBy)
            .ConfigureGuid(isRequired: true);

        builder.Property(u => u.ModifiedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(u => u.ModifiedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(u => u.IsDeleted)
            .ConfigureBoolean(isRequired: true);

        builder.Property(u => u.DeletedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(u => u.DeletedBy)
            .ConfigureGuid(isRequired: false);

        builder.HasIndex(u => u.Name)
            .HasDatabaseName("ix_units_name");

        builder.HasIndex(u => u.ShortName)
            .HasDatabaseName("ix_units_short_name");
    }
}
