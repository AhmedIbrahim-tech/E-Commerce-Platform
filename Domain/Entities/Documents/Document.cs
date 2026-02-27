using Domain.Common.Base;
using Domain.Common.Interfaces;
using Domain.Enums;

namespace Domain.Entities.Documents;

public class Document : BaseEntity, IAuditable, ISoftDelete
{
    public Guid UserId { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public DocumentStatus Status { get; private set; }

    public string FilePath { get; private set; } = string.Empty;
    public string FileName { get; private set; } = string.Empty;
    public string ContentType { get; private set; } = string.Empty;
    public long Size { get; private set; }

    public DateTimeOffset CreatedTime { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTimeOffset? ModifiedTime { get; private set; }
    public Guid? ModifiedBy { get; private set; }

    public bool IsDeleted { get; private set; }
    public DateTimeOffset? DeletedTime { get; private set; }
    public Guid? DeletedBy { get; private set; }

    private Document() { }

    public Document(Guid userId, string type, string fileName, string contentType, long size)
    {
        if (userId == Guid.Empty)
            throw new DomainException("User is required");

        if (string.IsNullOrWhiteSpace(fileName))
            throw new DomainException("File name is required");

        if (string.IsNullOrWhiteSpace(contentType))
            throw new DomainException("Content type is required");

        if (size <= 0)
            throw new DomainException("Invalid file size");

        UserId = userId;
        Type = string.IsNullOrWhiteSpace(type) ? "General" : type.Trim();
        Status = DocumentStatus.Pending;

        FileName = fileName.Trim();
        ContentType = contentType.Trim();
        Size = size;
    }

    public void AttachFile(string filePath, Guid modifiedBy)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new DomainException("File path is required");

        FilePath = filePath.Trim();
        ModifiedTime = DateTimeOffset.UtcNow;
        ModifiedBy = modifiedBy;
    }

    public void SetStatus(DocumentStatus status, Guid modifiedBy)
    {
        Status = status;
        ModifiedTime = DateTimeOffset.UtcNow;
        ModifiedBy = modifiedBy;
    }

    public void MarkDeleted(Guid userId)
    {
        IsDeleted = true;
        DeletedTime = DateTimeOffset.UtcNow;
        DeletedBy = userId;
    }
}

