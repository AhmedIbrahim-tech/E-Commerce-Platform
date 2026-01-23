namespace Application.Features.Tags.Queries.GetTagPaginatedList;

public class GetTagPaginatedListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetTagPaginatedListQuery, PaginatedResult<GetTagPaginatedListResponse>>
{
    public async Task<PaginatedResult<GetTagPaginatedListResponse>> Handle(GetTagPaginatedListQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Tag> queryable = unitOfWork.Tags.GetTableNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            queryable = queryable.Where(t => t.Name.Contains(request.Search!));

        queryable = queryable.OrderBy(t => t.Name);

        var totalCount = await queryable.CountAsync(cancellationToken);
        var tags = await queryable
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(t => new GetTagPaginatedListResponse(t.Id, t.Name, t.IsActive, t.CreatedTime))
            .ToListAsync(cancellationToken);

        var paginated = PaginatedResult<GetTagPaginatedListResponse>.Success(tags, totalCount, request.PageNumber, request.PageSize);
        paginated.Meta = new { Count = tags.Count };
        return paginated;
    }
}

