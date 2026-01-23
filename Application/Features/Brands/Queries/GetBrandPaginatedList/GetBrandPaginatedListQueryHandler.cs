using Application.Common.Bases;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Brands.Queries.GetBrandPaginatedList;

public class GetBrandPaginatedListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetBrandPaginatedListQuery, PaginatedResult<GetBrandPaginatedListResponse>>
{
    public async Task<PaginatedResult<GetBrandPaginatedListResponse>> Handle(GetBrandPaginatedListQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Brand, GetBrandPaginatedListResponse>> expression = b => new GetBrandPaginatedListResponse(
            b.Id,
            b.Name,
            b.Description,
            b.ImageUrl,
            b.IsActive,
            b.CreatedTime
        );

        var queryable = unitOfWork.Brands.GetTableNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            queryable = queryable.Where(b => b.Name.Contains(request.Search!) || 
                (b.Description != null && b.Description.Contains(request.Search!)));

        queryable = request.SortBy switch
        {
            BrandSortingEnum.NameAsc => queryable.OrderBy(b => b.Name),
            BrandSortingEnum.NameDesc => queryable.OrderByDescending(b => b.Name),
            BrandSortingEnum.CreatedTimeAsc => queryable.OrderBy(b => b.CreatedTime),
            BrandSortingEnum.CreatedTimeDesc => queryable.OrderByDescending(b => b.CreatedTime),
            _ => queryable.OrderBy(b => b.Name)
        };

        var paginatedList = await queryable.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
        paginatedList.Meta = new { Count = paginatedList.Data.Count() };
        return paginatedList;
    }
}
