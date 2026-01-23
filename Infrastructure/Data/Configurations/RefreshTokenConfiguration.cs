using Infrastructure.Data.Identity;

namespace Infrastructure.Data.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(u => u.AppUserId)
            .ConfigureGuid(isRequired: true);

        builder.Property(u => u.Token)
            .ConfigureString(500, isRequired: true);

        builder.Property(u => u.JwtId)
            .ConfigureString(200, isRequired: true);

        builder.Property(u => u.IsUsed)
            .ConfigureBoolean(defaultValue: false, isRequired: true);

        builder.Property(u => u.IsRevoked)
            .ConfigureBoolean(defaultValue: false, isRequired: true);

        builder.Property(u => u.CreatedAt)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        builder.Property(u => u.ExpiresAt)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: true);

        builder.Property(u => u.RevokedReason)
            .ConfigureString(500, isRequired: false);

        builder.Property(u => u.RevokedAt)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        // Foreign key relationship
        builder.HasOne(u => u.AppUser)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(u => u.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for performance
        builder.HasIndex(u => u.AppUserId)
            .HasDatabaseName("ix_user_refresh_tokens_app_user_id");

        builder.HasIndex(u => u.Token)
            .HasDatabaseName("ix_user_refresh_tokens_token");

        builder.HasIndex(u => u.JwtId)
            .HasDatabaseName("ix_user_refresh_tokens_jwt_id");

        builder.HasIndex(u => u.ExpiresAt)
            .HasDatabaseName("ix_user_refresh_tokens_expires_at");
    }
}
