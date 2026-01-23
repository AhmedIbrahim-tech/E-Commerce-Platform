namespace Application.Features.Documents.Queries.GetMyDocuments;

public record GetMyDocumentsQuery : IRequest<ApiResponse<List<GetMyDocumentsResponse>>>;

