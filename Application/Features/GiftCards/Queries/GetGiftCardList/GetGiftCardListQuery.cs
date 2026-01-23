using Application.Common.Bases;

namespace Application.Features.GiftCards.Queries.GetGiftCardList;

public record GetGiftCardListQuery() : IRequest<ApiResponse<List<GetGiftCardListResponse>>>;
