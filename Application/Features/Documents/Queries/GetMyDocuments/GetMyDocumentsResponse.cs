using Domain.Enums;

namespace Application.Features.Documents.Queries.GetMyDocuments;

public record GetMyDocumentsResponse(
    Guid Id,
    Guid UserId,
    string Type,
    DocumentStatus Status,
    string FileName,
    long Size,
    DateTimeOffset CreatedAt);

