namespace Application.Features.Documents.Queries.GetMyDocumentDownload;

public record GetMyDocumentDownloadQuery(Guid Id) : IRequest<ApiResponse<GetMyDocumentDownloadResponse>>;

