namespace Domain.Entities.Users;

public class Vendor : UserProfile
{
    public string? StoreName { get; private set; }
    public decimal CommissionRate { get; private set; }

    private Vendor() { }

    public Vendor(Guid appUserId,string ownerName,Gender gender,string storeName,decimal commissionRate,Guid createdBy): base(appUserId, ownerName, gender, createdBy)
    {
        if (commissionRate <= 0 || commissionRate >= 100)
            throw new DomainException("Invalid commission rate");

        StoreName = storeName.Trim();
        CommissionRate = commissionRate;
    }

    public void UpdateStoreName(string storeName, Guid modifiedBy)
    {
        if (string.IsNullOrWhiteSpace(storeName))
            throw new DomainException("Store name is required");

        StoreName = storeName.Trim();
        UpdateModifiedFields(modifiedBy);
    }

    public void UpdateCommissionRate(decimal commissionRate, Guid modifiedBy)
    {
        if (commissionRate <= 0 || commissionRate >= 100)
            throw new DomainException("Invalid commission rate");

        CommissionRate = commissionRate;
        UpdateModifiedFields(modifiedBy);
    }
}
