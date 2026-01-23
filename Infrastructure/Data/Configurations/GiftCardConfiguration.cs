using Domain.Entities.Promotions;

namespace Infrastructure.Data.Configurations;

public class GiftCardConfiguration : IEntityTypeConfiguration<GiftCard>
{
    public void Configure(EntityTypeBuilder<GiftCard> builder)
    {
        builder.HasKey(gc => gc.Id);

        builder.Property(gc => gc.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(gc => gc.Code)
            .ConfigureString(50, isRequired: true);

        builder.Property(gc => gc.RecipientName)
            .ConfigureString(200, isRequired: false);

        builder.Property(gc => gc.RecipientEmail)
            .ConfigureString(256, isRequired: false);

        builder.Property(gc => gc.Amount)
            .ConfigureDecimal(precision: 18, scale: 2, isRequired: true);

        builder.Property(gc => gc.RemainingAmount)
            .ConfigureDecimal(precision: 18, scale: 2, isRequired: true);

        builder.Property(gc => gc.ExpiryDate)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(gc => gc.IsActive)
            .ConfigureBoolean(defaultValue: true, isRequired: true);

        builder.Property(gc => gc.IsRedeemed)
            .ConfigureBoolean(defaultValue: false, isRequired: true);

        builder.Property(gc => gc.RedeemedDate)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(gc => gc.CreatedTime)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        builder.Property(gc => gc.CreatedBy)
            .ConfigureGuid(isRequired: true);

        builder.Property(gc => gc.ModifiedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(gc => gc.ModifiedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(gc => gc.IsDeleted)
            .ConfigureBoolean(isRequired: true);

        builder.Property(gc => gc.DeletedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(gc => gc.DeletedBy)
            .ConfigureGuid(isRequired: false);

        builder.HasIndex(gc => gc.Code)
            .IsUnique()
            .HasDatabaseName("ix_gift_cards_code");
    }
}
