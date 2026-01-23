using Application.Common.Bases;

namespace Application.Features.Accounts.Queries.GetAccountById;

public record GetAccountByIdQuery(Guid Id) : IRequest<ApiResponse<GetAccountByIdResponse>>;
