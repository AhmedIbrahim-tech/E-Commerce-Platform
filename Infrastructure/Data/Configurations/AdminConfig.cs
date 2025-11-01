using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class AdminConfig : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            // Configure TPT
            builder.ToTable("Admins");

            builder.Property(a => a.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.Gender)
                .HasConversion(
                Gndr => Gndr.ToString(),
                Gndr => (Gender)Enum.Parse(typeof(Gender), Gndr!));

            builder.Property(a => a.Email)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(a => a.PhoneNumber)
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(a => a.Address)
                .HasMaxLength(250)
                .IsRequired();
        }
    }
}
