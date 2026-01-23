using Application.Common.Bases;

namespace Application.Features.Accounts.Commands.DeleteAccount;

public record DeleteAccountCommand(Guid Id) : IRequest<ApiResponse<string>>;
