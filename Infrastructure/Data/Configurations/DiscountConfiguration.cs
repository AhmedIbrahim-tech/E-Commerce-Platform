using Domain.Entities.Promotions;

namespace Infrastructure.Data.Configurations;

public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(d => d.Name)
            .ConfigureString(100, isRequired: true);

        builder.Property(d => d.Description)
            .ConfigureString(300, isRequired: false);

        builder.Property(d => d.DiscountAmount)
            .ConfigureDecimal(precision: 18, scale: 2, isRequired: true);

        builder.Property(d => d.DiscountPercentage)
            .ConfigureDecimal(precision: 5, scale: 2, isRequired: false);

        builder.Property(d => d.StartDate)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: true);

        builder.Property(d => d.EndDate)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: true);

        builder.Property(d => d.IsActive)
            .ConfigureBoolean(defaultValue: true, isRequired: true);

        builder.Property(d => d.CreatedTime)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        builder.Property(d => d.CreatedBy)
            .ConfigureGuid(isRequired: true);

        builder.Property(d => d.ModifiedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(d => d.ModifiedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(d => d.IsDeleted)
            .ConfigureBoolean(isRequired: true);

        builder.Property(d => d.DeletedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(d => d.DeletedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(d => d.DiscountPlanId)
            .ConfigureGuid(isRequired: false);

        builder.HasOne(d => d.DiscountPlan)
            .WithMany(dp => dp.Discounts)
            .HasForeignKey(d => d.DiscountPlanId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(d => d.Name)
            .HasDatabaseName("ix_discounts_name");
    }
}

public class DiscountPlanConfiguration : IEntityTypeConfiguration<DiscountPlan>
{
    public void Configure(EntityTypeBuilder<DiscountPlan> builder)
    {
        builder.HasKey(dp => dp.Id);

        builder.Property(dp => dp.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(dp => dp.Name)
            .ConfigureString(100, isRequired: true);

        builder.Property(dp => dp.Description)
            .ConfigureString(300, isRequired: false);

        builder.Property(dp => dp.IsActive)
            .ConfigureBoolean(defaultValue: true, isRequired: true);

        builder.Property(dp => dp.CreatedTime)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        builder.Property(dp => dp.CreatedBy)
            .ConfigureGuid(isRequired: true);

        builder.Property(dp => dp.ModifiedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(dp => dp.ModifiedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(dp => dp.IsDeleted)
            .ConfigureBoolean(isRequired: true);

        builder.Property(dp => dp.DeletedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(dp => dp.DeletedBy)
            .ConfigureGuid(isRequired: false);

        builder.HasIndex(dp => dp.Name)
            .HasDatabaseName("ix_discount_plans_name");
    }
}
