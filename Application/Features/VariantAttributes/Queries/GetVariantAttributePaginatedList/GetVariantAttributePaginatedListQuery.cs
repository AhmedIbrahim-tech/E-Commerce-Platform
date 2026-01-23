namespace Application.Features.VariantAttributes.Queries.GetVariantAttributePaginatedList;

public record GetVariantAttributePaginatedListQuery(int PageNumber, int PageSize, string? Search,
    VariantAttributeSortingEnum SortBy) : IRequest<PaginatedResult<GetVariantAttributePaginatedListResponse>>;
