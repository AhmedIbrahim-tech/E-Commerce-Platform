namespace Application.Features.Units.Queries.GetUnitPaginatedList;

public class GetUnitPaginatedListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetUnitPaginatedListQuery, PaginatedResult<GetUnitPaginatedListResponse>>
{
    public async Task<PaginatedResult<GetUnitPaginatedListResponse>> Handle(GetUnitPaginatedListQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<UnitOfMeasure, GetUnitPaginatedListResponse>> expression = u => new GetUnitPaginatedListResponse(
            u.Id,
            u.Name,
            u.ShortName,
            u.Description,
            u.IsActive,
            u.CreatedTime
        );

        var queryable = unitOfWork.UnitOfMeasures.GetTableNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            queryable = queryable.Where(u => u.Name.Contains(request.Search!) ||
                u.ShortName.Contains(request.Search!) ||
                (u.Description != null && u.Description.Contains(request.Search!)));

        queryable = request.SortBy switch
        {
            UnitOfMeasureSortingEnum.NameAsc => queryable.OrderBy(u => u.Name),
            UnitOfMeasureSortingEnum.NameDesc => queryable.OrderByDescending(u => u.Name),
            UnitOfMeasureSortingEnum.CreatedTimeAsc => queryable.OrderBy(u => u.CreatedTime),
            UnitOfMeasureSortingEnum.CreatedTimeDesc => queryable.OrderByDescending(u => u.CreatedTime),
            _ => queryable.OrderBy(u => u.Name)
        };

        var paginatedList = await queryable.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
        paginatedList.Meta = new { Count = paginatedList.Data.Count() };
        return paginatedList;
    }
}
