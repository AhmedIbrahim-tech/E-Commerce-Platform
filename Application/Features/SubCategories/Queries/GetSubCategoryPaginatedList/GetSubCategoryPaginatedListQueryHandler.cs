using Application.Common.Bases;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.SubCategories.Queries.GetSubCategoryPaginatedList;

public class GetSubCategoryPaginatedListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetSubCategoryPaginatedListQuery, PaginatedResult<GetSubCategoryPaginatedListResponse>>
{
    public async Task<PaginatedResult<GetSubCategoryPaginatedListResponse>> Handle(GetSubCategoryPaginatedListQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<SubCategory, GetSubCategoryPaginatedListResponse>> expression = sc => new GetSubCategoryPaginatedListResponse(
            sc.Id,
            sc.Name,
            sc.Description,
            sc.ImageUrl,
            sc.Code,
            sc.CategoryId,
            sc.Category != null ? sc.Category.Name : null,
            sc.IsActive,
            sc.CreatedTime
        );

        var queryable = unitOfWork.SubCategories.GetTableNoTracking()
            .Include(sc => sc.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
            queryable = queryable.Where(sc => sc.Name.Contains(request.Search!) || 
                (sc.Description != null && sc.Description.Contains(request.Search!)) ||
                (sc.Code != null && sc.Code.Contains(request.Search!)));

        if (request.CategoryId.HasValue)
            queryable = queryable.Where(sc => sc.CategoryId == request.CategoryId.Value);

        queryable = request.SortBy switch
        {
            SubCategorySortingEnum.NameAsc => queryable.OrderBy(sc => sc.Name),
            SubCategorySortingEnum.NameDesc => queryable.OrderByDescending(sc => sc.Name),
            SubCategorySortingEnum.CreatedTimeAsc => queryable.OrderBy(sc => sc.CreatedTime),
            SubCategorySortingEnum.CreatedTimeDesc => queryable.OrderByDescending(sc => sc.CreatedTime),
            _ => queryable.OrderBy(sc => sc.Name)
        };

        var paginatedList = await queryable.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
        paginatedList.Meta = new { Count = paginatedList.Data.Count() };
        return paginatedList;
    }
}
