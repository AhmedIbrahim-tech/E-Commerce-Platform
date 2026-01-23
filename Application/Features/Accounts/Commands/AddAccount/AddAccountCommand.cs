using Application.Common.Bases;

namespace Application.Features.Accounts.Commands.AddAccount;

public record AddAccountCommand(string AccountName, string AccountNumber, string? BankName, string? BranchName,
    string? Iban, string? SwiftCode, decimal InitialBalance, string? Description, bool IsActive) : IRequest<ApiResponse<string>>;
