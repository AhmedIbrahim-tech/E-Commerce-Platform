namespace Application.Features.Accounts.Queries.GetAccountList;

public record GetAccountListResponse(Guid Id, string AccountName, string AccountNumber, string? BankName,
    string? BranchName, string? Iban, string? SwiftCode, decimal InitialBalance, decimal CurrentBalance,
    bool IsActive, DateTimeOffset CreatedTime);
