namespace Domain.Common.Interfaces;

public interface ISoftDelete
{
    bool IsDeleted { get; }
    DateTimeOffset? DeletedTime { get; }
    Guid? DeletedBy { get; }

    void MarkDeleted(Guid userId);
}
