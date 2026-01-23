using Application.Common.Bases;

namespace Application.Features.Accounts.Queries.GetAccountList;

public record GetAccountListQuery() : IRequest<ApiResponse<List<GetAccountListResponse>>>;
