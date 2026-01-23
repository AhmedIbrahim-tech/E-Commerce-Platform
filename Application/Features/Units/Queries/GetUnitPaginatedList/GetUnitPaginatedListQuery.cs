namespace Application.Features.Units.Queries.GetUnitPaginatedList;

public record GetUnitPaginatedListQuery(int PageNumber, int PageSize, string? Search,
    UnitOfMeasureSortingEnum SortBy) : IRequest<PaginatedResult<GetUnitPaginatedListResponse>>;
