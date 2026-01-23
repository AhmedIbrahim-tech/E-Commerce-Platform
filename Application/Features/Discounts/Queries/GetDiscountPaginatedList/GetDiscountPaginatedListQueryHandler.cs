using Application.Common.Bases;
using Domain.Entities.Promotions;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Discounts.Queries.GetDiscountPaginatedList;

public class GetDiscountPaginatedListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(), IRequestHandler<GetDiscountPaginatedListQuery, PaginatedResult<GetDiscountPaginatedListResponse>>
{
    public async Task<PaginatedResult<GetDiscountPaginatedListResponse>> Handle(GetDiscountPaginatedListQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Discount, GetDiscountPaginatedListResponse>> expression = d => new GetDiscountPaginatedListResponse(
            d.Id,
            d.Name,
            d.Description,
            d.DiscountAmount,
            d.DiscountPercentage,
            d.StartDate,
            d.EndDate,
            d.DiscountPlanId,
            d.DiscountPlan != null ? d.DiscountPlan.Name : null,
            d.IsActive,
            d.CreatedTime
        );

        IQueryable<Discount> queryable = unitOfWork.Discounts.GetTableNoTracking()
            .Include(d => d.DiscountPlan);

        if (!string.IsNullOrWhiteSpace(request.Search))
            queryable = queryable.Where(d => d.Name.Contains(request.Search!) || 
                (d.Description != null && d.Description.Contains(request.Search!)));

        queryable = request.SortBy switch
        {
            DiscountSortingEnum.NameAsc => queryable.OrderBy(d => d.Name),
            DiscountSortingEnum.NameDesc => queryable.OrderByDescending(d => d.Name),
            DiscountSortingEnum.CreatedTimeAsc => queryable.OrderBy(d => d.CreatedTime),
            DiscountSortingEnum.CreatedTimeDesc => queryable.OrderByDescending(d => d.CreatedTime),
            _ => queryable.OrderBy(d => d.Name)
        };

        var paginatedList = await queryable.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
        paginatedList.Meta = new { Count = paginatedList.Data.Count() };
        return paginatedList;
    }
}
