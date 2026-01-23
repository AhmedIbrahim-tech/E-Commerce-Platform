using Domain.Common.Base;
using Domain.Common.Interfaces;

namespace Domain.Entities.Accounts;

public class Account : BaseEntity, IAuditable, ISoftDelete
{
    public string AccountName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string? BankName { get; set; }
    public string? BranchName { get; set; }
    public string? Iban { get; set; }
    public string? SwiftCode { get; set; }
    public decimal InitialBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedTime { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset? ModifiedTime { get; set; }
    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }
    public Guid? DeletedBy { get; set; }

    public Account()
    {
        CurrentBalance = InitialBalance;
        CreatedTime = DateTimeOffset.UtcNow;
    }

    public void MarkDeleted(Guid userId)
    {
        IsDeleted = true;
        DeletedTime = DateTimeOffset.UtcNow;
        DeletedBy = userId;
    }
}
