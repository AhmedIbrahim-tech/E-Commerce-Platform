using Domain.Common.Base;
using Domain.Common.Interfaces;

namespace Domain.Entities.Catalog;

public class Tag : BaseEntity, IAuditable, ISoftDelete
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedTime { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset? ModifiedTime { get; set; }
    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }
    public Guid? DeletedBy { get; set; }

    public ICollection<ProductTag> ProductTags { get; set; }

    public Tag()
    {
        ProductTags = new HashSet<ProductTag>();
        CreatedTime = DateTimeOffset.UtcNow;
    }

    public void MarkDeleted(Guid userId)
    {
        IsDeleted = true;
        DeletedTime = DateTimeOffset.UtcNow;
        DeletedBy = userId;
    }
}

