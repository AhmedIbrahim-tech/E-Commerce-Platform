namespace Domain.Entities.Users;

public abstract class UserProfile : BaseEntity, IAuditable, ISoftDelete
{
    public Guid AppUserId { get; private set; }

    public string? FullName { get; private set; }
    public Gender Gender { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? SecondPhoneNumber { get; private set; }

    // Audit
    public DateTimeOffset CreatedTime { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTimeOffset? ModifiedTime { get; private set; }
    public Guid? ModifiedBy { get; private set; }

    // Soft Delete
    public bool IsDeleted { get; private set; }
    public DateTimeOffset? DeletedTime { get; private set; }
    public Guid? DeletedBy { get; private set; }

    protected UserProfile(Guid appUserId, string fullName, Gender gender, Guid createdBy)
    {
        SetName(fullName);

        AppUserId = appUserId;
        Gender = gender;

        CreatedTime = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }

    protected UserProfile()
    {
    }

    public void ChangeName(string fullName, Guid modifiedBy)
    {
        SetName(fullName);
        UpdateModifiedFields(modifiedBy);
    }

    public void ChangeGender(Gender gender, Guid modifiedBy)
    {
        Gender = gender;
        UpdateModifiedFields(modifiedBy);
    }

    public void ChangePhoneNumber(string? phoneNumber, Guid modifiedBy)
    {
        PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber.Trim();
        UpdateModifiedFields(modifiedBy);
    }

    public void ChangeSecondPhoneNumber(string? secondPhoneNumber, Guid modifiedBy)
    {
        SecondPhoneNumber = string.IsNullOrWhiteSpace(secondPhoneNumber) ? null : secondPhoneNumber.Trim();
        UpdateModifiedFields(modifiedBy);
    }

    protected void UpdateModifiedFields(Guid modifiedBy)
    {
        ModifiedTime = DateTimeOffset.UtcNow;
        ModifiedBy = modifiedBy;
    }

    public void MarkDeleted(Guid userId)
    {
        IsDeleted = true;
        DeletedTime = DateTimeOffset.UtcNow;
        DeletedBy = userId;
    }

    public void Restore(Guid userId)
    {
        IsDeleted = false;
        DeletedTime = null;
        DeletedBy = null;
        UpdateModifiedFields(userId);
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Full name is required");

        FullName = name.Trim();
    }


}
