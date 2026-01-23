using Domain.Entities.Promotions;

namespace Infrastructure.Data.Configurations;

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(c => c.Code)
            .ConfigureString(50, isRequired: true);

        builder.Property(c => c.Name)
            .ConfigureString(100, isRequired: true);

        builder.Property(c => c.Description)
            .ConfigureString(300, isRequired: false);

        builder.Property(c => c.DiscountAmount)
            .ConfigureDecimal(precision: 18, scale: 2, isRequired: true);

        builder.Property(c => c.DiscountPercentage)
            .ConfigureDecimal(precision: 5, scale: 2, isRequired: false);

        builder.Property(c => c.MinimumPurchaseAmount)
            .ConfigureDecimal(precision: 18, scale: 2, isRequired: false);

        builder.Property(c => c.MaximumDiscountAmount)
            .ConfigureDecimal(precision: 18, scale: 2, isRequired: false);

        builder.Property(c => c.StartDate)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: true);

        builder.Property(c => c.EndDate)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: true);

        builder.Property(c => c.UsageLimit);

        builder.Property(c => c.UsedCount)
            .HasDefaultValue(0);

        builder.Property(c => c.IsActive)
            .ConfigureBoolean(defaultValue: true, isRequired: true);

        builder.Property(c => c.CreatedTime)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        builder.Property(c => c.CreatedBy)
            .ConfigureGuid(isRequired: true);

        builder.Property(c => c.ModifiedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(c => c.ModifiedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(c => c.IsDeleted)
            .ConfigureBoolean(isRequired: true);

        builder.Property(c => c.DeletedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(c => c.DeletedBy)
            .ConfigureGuid(isRequired: false);

        builder.HasIndex(c => c.Code)
            .IsUnique()
            .HasDatabaseName("ix_coupons_code");
    }
}
