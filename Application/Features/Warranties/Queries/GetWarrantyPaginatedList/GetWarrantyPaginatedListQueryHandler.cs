using Application.Common.Bases;

namespace Application.Features.Warranties.Queries.GetWarrantyPaginatedList;

public class GetWarrantyPaginatedListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetWarrantyPaginatedListQuery, PaginatedResult<GetWarrantyPaginatedListResponse>>
{
    public async Task<PaginatedResult<GetWarrantyPaginatedListResponse>> Handle(GetWarrantyPaginatedListQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Warranty, GetWarrantyPaginatedListResponse>> expression = w => new GetWarrantyPaginatedListResponse(
            w.Id,
            w.Name,
            w.Description,
            w.Duration,
            w.DurationPeriod,
            w.IsActive
        );

        var queryable = unitOfWork.Warranties.GetTableNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            queryable = queryable.Where(w => w.Name.Contains(request.Search!) || 
                                           (w.Description != null && w.Description.Contains(request.Search!)));

        queryable = queryable.OrderBy(w => w.Name);

        var paginatedList = await queryable.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
        paginatedList.Meta = new { Count = paginatedList.Data.Count() };
        return paginatedList;
    }
}
