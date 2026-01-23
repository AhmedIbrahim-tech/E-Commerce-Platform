namespace Domain.Common.Interfaces;

public interface IAuditable
{
    DateTimeOffset CreatedTime { get; }
    Guid CreatedBy { get; }

    DateTimeOffset? ModifiedTime { get; }
    Guid? ModifiedBy { get; }
}
