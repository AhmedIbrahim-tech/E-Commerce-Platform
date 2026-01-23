namespace Domain.Entities.Users;

public class Admin : UserProfile
{
    public string? Address { get; private set; }

    private Admin() { }

    public Admin(Guid appUserId, string fullName, Gender gender, string address, Guid createdBy) : base(appUserId, fullName, gender, createdBy)
    {
        SetAddress(address);
    }

    public void SetAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new DomainException("Address is required");

        Address = address.Trim();
    }

    public void ChangeAddress(string address, Guid modifiedBy)
    {
        SetAddress(address);
        UpdateModifiedFields(modifiedBy);
    }

}
