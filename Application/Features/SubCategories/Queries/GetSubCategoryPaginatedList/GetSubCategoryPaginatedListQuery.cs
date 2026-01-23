namespace Application.Features.SubCategories.Queries.GetSubCategoryPaginatedList;

public record GetSubCategoryPaginatedListQuery(int PageNumber, int PageSize, string? Search, Guid? CategoryId,
    SubCategorySortingEnum SortBy) : IRequest<PaginatedResult<GetSubCategoryPaginatedListResponse>>;
