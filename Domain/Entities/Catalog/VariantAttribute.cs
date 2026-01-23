using Domain.Common.Base;
using Domain.Common.Interfaces;

namespace Domain.Entities.Catalog;

public class VariantAttribute : BaseEntity, IAuditable, ISoftDelete
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedTime { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset? ModifiedTime { get; set; }
    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }
    public Guid? DeletedBy { get; set; }

    public ICollection<VariantAttributeValue> Values { get; set; }

    public VariantAttribute()
    {
        Values = new HashSet<VariantAttributeValue>();
        CreatedTime = DateTimeOffset.UtcNow;
    }

    public void MarkDeleted(Guid userId)
    {
        IsDeleted = true;
        DeletedTime = DateTimeOffset.UtcNow;
        DeletedBy = userId;
    }
}

public class VariantAttributeValue : BaseEntity, IAuditable, ISoftDelete
{
    public string Value { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedTime { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset? ModifiedTime { get; set; }
    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }
    public Guid? DeletedBy { get; set; }

    public Guid VariantAttributeId { get; set; }
    public VariantAttribute? VariantAttribute { get; set; }

    public VariantAttributeValue()
    {
        CreatedTime = DateTimeOffset.UtcNow;
    }

    public void MarkDeleted(Guid userId)
    {
        IsDeleted = true;
        DeletedTime = DateTimeOffset.UtcNow;
        DeletedBy = userId;
    }
}
