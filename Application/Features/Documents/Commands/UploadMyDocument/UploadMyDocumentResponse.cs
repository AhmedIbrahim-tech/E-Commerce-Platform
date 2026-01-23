using Domain.Enums;

namespace Application.Features.Documents.Commands.UploadMyDocument;

public record UploadMyDocumentResponse(
    Guid Id,
    Guid UserId,
    string Type,
    DocumentStatus Status,
    string FileName,
    long Size,
    DateTimeOffset CreatedAt);

