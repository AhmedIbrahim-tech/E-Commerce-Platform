namespace Application.Features.Tags.Queries.GetTagById;

public class GetTagByIdQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetTagByIdQuery, ApiResponse<GetTagByIdResponse>>
{
    public async Task<ApiResponse<GetTagByIdResponse>> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
    {
        var tag = await unitOfWork.Tags.GetTableNoTracking()
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (tag is null)
            return NotFound<GetTagByIdResponse>("Tag not found");

        return Success(new GetTagByIdResponse(tag.Id, tag.Name, tag.IsActive, tag.CreatedTime, tag.ModifiedTime));
    }
}

