namespace Application.Features.Tags.Queries.GetTagPaginatedList;

public record GetTagPaginatedListQuery(int PageNumber, int PageSize, string? Search)
    : IRequest<PaginatedResult<GetTagPaginatedListResponse>>;

