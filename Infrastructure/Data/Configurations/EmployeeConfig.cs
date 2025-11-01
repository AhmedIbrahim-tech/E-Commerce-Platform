using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            // Configure TPT
            builder.ToTable("Employees");

            builder.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Gender)
                .HasConversion(
                Gndr => Gndr.ToString(),
                Gndr => (Gender)Enum.Parse(typeof(Gender), Gndr!));

            builder.Property(e => e.Email)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(e => e.Address)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(e => e.Position)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Salary)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(e => e.HireDate)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()")
                .IsRequired();
        }
    }
}
