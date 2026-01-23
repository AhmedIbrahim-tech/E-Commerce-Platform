using Application.Common.Bases;

namespace Application.Features.GiftCards.Commands.EditGiftCard;

public record EditGiftCardCommand(Guid Id, string Code, string? RecipientName, string? RecipientEmail, decimal Amount,
    DateTimeOffset? ExpiryDate, bool IsActive) : IRequest<ApiResponse<string>>;
