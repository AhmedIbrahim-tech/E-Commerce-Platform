using Application.Common.Bases;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Products.Queries.GetProductPaginatedList;

public class GetProductPaginatedListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetProductPaginatedListQuery, PaginatedResult<GetProductPaginatedListResponse>>
{
    public async Task<PaginatedResult<GetProductPaginatedListResponse>> Handle(GetProductPaginatedListQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Product, GetProductPaginatedListResponse>> expression = c => new GetProductPaginatedListResponse
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            Price = c.Price,
            StockQuantity = c.StockQuantity,
            ImageURL = c.ImageURL,
            CreatedAt = c.CreatedAt,
            CategoryName = c.Category!.Name,
        };

        var queryable = unitOfWork.Products.GetTableNoTracking()
            .Include(p => p.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
            queryable = queryable.Where(c => c.Name!.Contains(request.Search!) || c.Description!.Contains(request.Search!));

        queryable = request.SortBy switch
        {
            ProductSortingEnum.NameAsc => queryable.OrderBy(c => c.Name),
            ProductSortingEnum.NameDesc => queryable.OrderByDescending(c => c.Name),
            ProductSortingEnum.PriceAsc => queryable.OrderBy(c => c.Price),
            ProductSortingEnum.PriceDesc => queryable.OrderByDescending(c => c.Price),
            ProductSortingEnum.StockQuantityAsc => queryable.OrderBy(c => c.StockQuantity),
            ProductSortingEnum.StockQuantityDesc => queryable.OrderByDescending(c => c.StockQuantity),
            ProductSortingEnum.CreatedDateAsc => queryable.OrderBy(c => c.CreatedAt),
            ProductSortingEnum.CreatedDateDesc => queryable.OrderByDescending(c => c.CreatedAt),
            ProductSortingEnum.RatingAsc => queryable.OrderBy(c => c.Reviews.Any() ? c.Reviews.Max(r => r.Rating) : 0),
            ProductSortingEnum.RatingDesc => queryable.OrderByDescending(c => c.Reviews.Any() ? c.Reviews.Max(r => r.Rating) : 0),
            _ => queryable.OrderBy(c => c.Name)
        };

        var paginatedList = await queryable.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
        paginatedList.Meta = new { Count = paginatedList.Data.Count() };
        return paginatedList;
    }
}

