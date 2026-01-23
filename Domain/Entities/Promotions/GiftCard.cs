using Domain.Common.Base;
using Domain.Common.Interfaces;

namespace Domain.Entities.Promotions;

public class GiftCard : BaseEntity, IAuditable, ISoftDelete
{
    public string Code { get; set; } = string.Empty;
    public string? RecipientName { get; set; }
    public string? RecipientEmail { get; set; }
    public decimal Amount { get; set; }
    public decimal RemainingAmount { get; set; }
    public DateTimeOffset? ExpiryDate { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsRedeemed { get; set; } = false;
    public DateTimeOffset? RedeemedDate { get; set; }

    public DateTimeOffset CreatedTime { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset? ModifiedTime { get; set; }
    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }
    public Guid? DeletedBy { get; set; }

    public GiftCard()
    {
        RemainingAmount = Amount;
        CreatedTime = DateTimeOffset.UtcNow;
    }

    public void MarkDeleted(Guid userId)
    {
        IsDeleted = true;
        DeletedTime = DateTimeOffset.UtcNow;
        DeletedBy = userId;
    }
}
