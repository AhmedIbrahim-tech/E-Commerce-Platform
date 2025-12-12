namespace Infrastructure.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("employees");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .ConfigureGuid("id", isRequired: true);

        builder.Property(e => e.FullName)
            .ConfigureString("full_name", 200, isRequired: false);

        builder.Property(e => e.Gender)
            .ConfigureEnum("gender", isRequired: false);

        builder.Property(e => e.Address)
            .ConfigureString("address", 250, isRequired: false);

        builder.Property(e => e.Position)
            .ConfigureString("position", 100, isRequired: false);

        builder.Property(e => e.Salary)
            .ConfigureDecimal("salary", precision: 18, scale: 2, isRequired: false);

        builder.Property(e => e.HireDate)
            .ConfigureTimestamp("hire_date", hasDefaultValue: true, isRequired: false);

        builder.Property(e => e.AppUserId)
            .ConfigureGuid("app_user_id", isRequired: true);

        // Indexes for performance
        builder.HasIndex(e => e.AppUserId)
            .HasDatabaseName("ix_employees_app_user_id")
            .IsUnique();

        builder.HasIndex(e => e.Position)
            .HasDatabaseName("ix_employees_position");
    }
}
