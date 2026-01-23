using Domain.Entities.Catalog;

namespace Infrastructure.Data.Configurations;

public class WarrantyConfiguration : IEntityTypeConfiguration<Warranty>
{
    public void Configure(EntityTypeBuilder<Warranty> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(w => w.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(w => w.Description)
            .ConfigureString(500, isRequired: false);

        builder.Property(w => w.Duration)
            .ConfigureInteger(isRequired: true);

        builder.Property(w => w.DurationPeriod)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(w => w.IsActive)
            .ConfigureBoolean(defaultValue: true, isRequired: true);

        builder.Property(w => w.CreatedTime)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        builder.Property(w => w.CreatedBy)
            .ConfigureGuid(isRequired: true);

        builder.Property(w => w.ModifiedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(w => w.ModifiedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(w => w.IsDeleted)
            .ConfigureBoolean(isRequired: true);

        builder.Property(w => w.DeletedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(w => w.DeletedBy)
            .ConfigureGuid(isRequired: false);

        builder.HasIndex(w => w.Name)
            .HasDatabaseName("ix_warranties_name");
    }
}
