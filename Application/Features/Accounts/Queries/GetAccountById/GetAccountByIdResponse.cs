namespace Application.Features.Accounts.Queries.GetAccountById;

public record GetAccountByIdResponse(Guid Id, string AccountName, string AccountNumber, string? BankName,
    string? BranchName, string? Iban, string? SwiftCode, decimal InitialBalance, decimal CurrentBalance,
    string? Description, bool IsActive, DateTimeOffset CreatedTime);
