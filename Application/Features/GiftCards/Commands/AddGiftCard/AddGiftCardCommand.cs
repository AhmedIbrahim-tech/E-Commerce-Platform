using Application.Common.Bases;

namespace Application.Features.GiftCards.Commands.AddGiftCard;

public record AddGiftCardCommand(string Code, string? RecipientName, string? RecipientEmail, decimal Amount,
    DateTimeOffset? ExpiryDate, bool IsActive) : IRequest<ApiResponse<string>>;
