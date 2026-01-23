using Application.Common.Bases;

namespace Application.Features.Discounts.Queries.GetDiscountList;

public record GetDiscountListQuery() : IRequest<ApiResponse<List<GetDiscountListResponse>>>;
