using Application.Common.Bases;

namespace Application.Features.GiftCards.Commands.DeleteGiftCard;

public record DeleteGiftCardCommand(Guid Id) : IRequest<ApiResponse<string>>;
