namespace Application.Features.GiftCards.Queries.GetGiftCardById;

public record GetGiftCardByIdResponse(Guid Id, string Code, string? RecipientName, string? RecipientEmail,
    decimal Amount, decimal RemainingAmount, DateTimeOffset? ExpiryDate, bool IsActive, bool IsRedeemed,
    DateTimeOffset? RedeemedDate, DateTimeOffset CreatedTime);
