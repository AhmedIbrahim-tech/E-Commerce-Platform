using Domain.Entities.Documents;

namespace Application.Features.Documents.Queries.GetMyDocuments;

public class GetMyDocumentsQueryHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<GetMyDocumentsQuery, ApiResponse<List<GetMyDocumentsResponse>>>
{
    public async Task<ApiResponse<List<GetMyDocumentsResponse>>> Handle(GetMyDocumentsQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserId();

        var documents = await unitOfWork.Documents.GetTableNoTracking()
            .Where(d => d.UserId == userId)
            .OrderByDescending(d => d.CreatedTime)
            .Select(d => new GetMyDocumentsResponse(
                d.Id,
                d.UserId,
                d.Type,
                d.Status,
                d.FileName,
                d.Size,
                d.CreatedTime))
            .ToListAsync(cancellationToken);

        return Success(documents);
    }
}

