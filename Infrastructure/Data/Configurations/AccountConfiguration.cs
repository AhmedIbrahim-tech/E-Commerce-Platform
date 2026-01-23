using Domain.Entities.Accounts;

namespace Infrastructure.Data.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(a => a.AccountName)
            .ConfigureString(200, isRequired: true);

        builder.Property(a => a.AccountNumber)
            .ConfigureString(50, isRequired: true);

        builder.Property(a => a.BankName)
            .ConfigureString(200, isRequired: false);

        builder.Property(a => a.BranchName)
            .ConfigureString(200, isRequired: false);

        builder.Property(a => a.Iban)
            .ConfigureString(50, isRequired: false);

        builder.Property(a => a.SwiftCode)
            .ConfigureString(20, isRequired: false);

        builder.Property(a => a.InitialBalance)
            .ConfigureDecimal(precision: 18, scale: 2, isRequired: true);

        builder.Property(a => a.CurrentBalance)
            .ConfigureDecimal(precision: 18, scale: 2, isRequired: true);

        builder.Property(a => a.Description)
            .ConfigureString(300, isRequired: false);

        builder.Property(a => a.IsActive)
            .ConfigureBoolean(defaultValue: true, isRequired: true);

        builder.Property(a => a.CreatedTime)
            .ConfigureTimestamp(hasDefaultValue: true, isRequired: true);

        builder.Property(a => a.CreatedBy)
            .ConfigureGuid(isRequired: true);

        builder.Property(a => a.ModifiedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(a => a.ModifiedBy)
            .ConfigureGuid(isRequired: false);

        builder.Property(a => a.IsDeleted)
            .ConfigureBoolean(isRequired: true);

        builder.Property(a => a.DeletedTime)
            .ConfigureTimestamp(hasDefaultValue: false, isRequired: false);

        builder.Property(a => a.DeletedBy)
            .ConfigureGuid(isRequired: false);

        builder.HasIndex(a => a.AccountNumber)
            .HasDatabaseName("ix_accounts_account_number");
    }
}
