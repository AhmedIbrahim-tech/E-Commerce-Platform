using Domain.Entities.Documents;

namespace Application.Features.Documents.Queries.GetMyDocumentDownload;

public class GetMyDocumentDownloadQueryHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<GetMyDocumentDownloadQuery, ApiResponse<GetMyDocumentDownloadResponse>>
{
    public async Task<ApiResponse<GetMyDocumentDownloadResponse>> Handle(GetMyDocumentDownloadQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserId();

        var document = await unitOfWork.Documents.GetTableNoTracking()
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (document == null)
            return NotFound<GetMyDocumentDownloadResponse>("Document not found");

        if (document.UserId != userId)
            return Unauthorized<GetMyDocumentDownloadResponse>("You do not have permission to download this document");

        if (string.IsNullOrWhiteSpace(document.FilePath))
            return BadRequest<GetMyDocumentDownloadResponse>("Document file is missing");

        var response = new GetMyDocumentDownloadResponse
        {
            RelativePath = document.FilePath,
            FileName = document.FileName,
            ContentType = document.ContentType
        };

        return Success(response);
    }
}

