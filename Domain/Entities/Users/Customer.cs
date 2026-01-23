namespace Domain.Entities.Users;

public class Customer : UserProfile
{
    private Customer() { }

    public Customer(Guid appUserId, string fullName, Gender gender, Guid createdBy) : base(appUserId, fullName, gender, createdBy)
    {
    }
}
