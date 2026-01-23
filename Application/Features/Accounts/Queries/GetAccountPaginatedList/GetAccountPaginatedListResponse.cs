namespace Application.Features.Accounts.Queries.GetAccountPaginatedList;

public record GetAccountPaginatedListResponse(Guid Id, string AccountName, string AccountNumber, string? BankName,
    string? BranchName, decimal InitialBalance, decimal CurrentBalance, bool IsActive, DateTimeOffset CreatedTime);
