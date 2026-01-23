namespace Application.Features.GiftCards.Queries.GetGiftCardList;

public record GetGiftCardListResponse(Guid Id, string Code, string? RecipientName, string? RecipientEmail, 
    decimal Amount, decimal RemainingAmount, DateTimeOffset? ExpiryDate, bool IsActive, bool IsRedeemed, DateTimeOffset CreatedTime);
