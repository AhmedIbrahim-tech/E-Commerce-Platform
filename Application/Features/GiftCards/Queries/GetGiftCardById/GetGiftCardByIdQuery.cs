using Application.Common.Bases;

namespace Application.Features.GiftCards.Queries.GetGiftCardById;

public record GetGiftCardByIdQuery(Guid Id) : IRequest<ApiResponse<GetGiftCardByIdResponse>>;
