namespace Infrastructure.Data.Configurations;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.ToTable("admins");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .ValueGeneratedNever()
            .ConfigureGuid("id", isRequired: true);

        builder.Property(a => a.FullName)
            .ConfigureString("full_name", 200, isRequired: false);

        builder.Property(a => a.Gender)
            .ConfigureEnum("gender", isRequired: false);

        builder.Property(a => a.Address)
            .ConfigureString("address", 250, isRequired: false);

        builder.Property(a => a.AppUserId)
            .ConfigureGuid("app_user_id", isRequired: true);

        // Index for AppUserId lookup
        builder.HasIndex(a => a.AppUserId)
            .HasDatabaseName("ix_admins_app_user_id")
            .IsUnique();
    }
}
