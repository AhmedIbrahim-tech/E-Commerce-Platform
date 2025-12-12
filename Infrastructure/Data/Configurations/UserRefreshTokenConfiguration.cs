using Infrastructure.Data.Identity;

namespace Infrastructure.Data.Configurations;

public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
{
    public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
    {
        builder.ToTable("user_refresh_tokens");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedNever()
            .ConfigureGuid("id", isRequired: true);

        builder.Property(u => u.UserId)
            .ConfigureGuid("user_id", isRequired: true);

        builder.Property(u => u.Token)
            .ConfigureString("token", 500, isRequired: false);

        builder.Property(u => u.RefreshToken)
            .ConfigureString("refresh_token", 500, isRequired: false);

        builder.Property(u => u.JwtId)
            .ConfigureString("jwt_id", 200, isRequired: false);

        builder.Property(u => u.IsUsed)
            .ConfigureBoolean("is_used", defaultValue: false, isRequired: true);

        builder.Property(u => u.IsRevoked)
            .ConfigureBoolean("is_revoked", defaultValue: false, isRequired: true);

        builder.Property(u => u.AddedTime)
            .ConfigureTimestamp("added_time", hasDefaultValue: true, isRequired: true);

        builder.Property(u => u.ExpiryDate)
            .ConfigureTimestamp("expiry_date", hasDefaultValue: false, isRequired: true);

        // Foreign key relationship
        builder.HasOne<AppUser>()
            .WithMany(u => u.UserRefreshTokens)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for performance
        builder.HasIndex(u => u.UserId)
            .HasDatabaseName("ix_user_refresh_tokens_user_id");

        builder.HasIndex(u => u.RefreshToken)
            .HasDatabaseName("ix_user_refresh_tokens_refresh_token");

        builder.HasIndex(u => u.JwtId)
            .HasDatabaseName("ix_user_refresh_tokens_jwt_id");

        builder.HasIndex(u => u.ExpiryDate)
            .HasDatabaseName("ix_user_refresh_tokens_expiry_date");
    }
}
