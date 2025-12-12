using Application.Common.Bases;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Categories.Queries.GetCategoryPaginatedList;

public class GetCategoryPaginatedListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetCategoryPaginatedListQuery, PaginatedResult<GetCategoryPaginatedListResponse>>
{
    public async Task<PaginatedResult<GetCategoryPaginatedListResponse>> Handle(GetCategoryPaginatedListQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Category, GetCategoryPaginatedListResponse>> expression = c => new GetCategoryPaginatedListResponse(
            c.Id,
            c.Name!,
            c.Description
        );

        var queryable = unitOfWork.Categories.GetTableNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            queryable = queryable.Where(c => c.Name!.Contains(request.Search!) || c.Description!.Contains(request.Search!));

        queryable = request.SortBy switch
        {
            CategorySortingEnum.NameAsc => queryable.OrderBy(c => c.Name),
            CategorySortingEnum.NameDesc => queryable.OrderByDescending(c => c.Name),
            _ => queryable.OrderBy(c => c.Name)
        };

        var paginatedList = await queryable.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
        paginatedList.Meta = new { Count = paginatedList.Data.Count() };
        return paginatedList;
    }
}

