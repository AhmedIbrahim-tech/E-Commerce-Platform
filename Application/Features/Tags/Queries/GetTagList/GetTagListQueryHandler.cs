namespace Application.Features.Tags.Queries.GetTagList;

public class GetTagListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetTagListQuery, ApiResponse<List<GetTagListResponse>>>
{
    public async Task<ApiResponse<List<GetTagListResponse>>> Handle(GetTagListQuery request, CancellationToken cancellationToken)
    {
        var tags = await unitOfWork.Tags.GetTableNoTracking()
            .OrderBy(t => t.Name)
            .Select(t => new GetTagListResponse(
                t.Id,
                t.Name,
                t.IsActive,
                t.CreatedTime))
            .ToListAsync(cancellationToken);

        return Success(tags);
    }
}

