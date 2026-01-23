using Application.Common.Bases;
using Domain.Entities.Promotions;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Coupons.Queries.GetCouponPaginatedList;

public class GetCouponPaginatedListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetCouponPaginatedListQuery, PaginatedResult<GetCouponPaginatedListResponse>>
{
    public async Task<PaginatedResult<GetCouponPaginatedListResponse>> Handle(GetCouponPaginatedListQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Coupon, GetCouponPaginatedListResponse>> expression = c => new GetCouponPaginatedListResponse(
            c.Id,
            c.Code,
            c.Name,
            c.Description,
            c.DiscountAmount,
            c.DiscountPercentage,
            c.StartDate,
            c.EndDate,
            c.UsageLimit,
            c.UsedCount,
            c.IsActive,
            c.CreatedTime
        );

        var queryable = unitOfWork.Coupons.GetTableNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            queryable = queryable.Where(c => c.Code.Contains(request.Search!) || 
                c.Name.Contains(request.Search!) ||
                (c.Description != null && c.Description.Contains(request.Search!)));

        queryable = request.SortBy switch
        {
            CouponSortingEnum.CodeAsc => queryable.OrderBy(c => c.Code),
            CouponSortingEnum.CodeDesc => queryable.OrderByDescending(c => c.Code),
            CouponSortingEnum.NameAsc => queryable.OrderBy(c => c.Name),
            CouponSortingEnum.NameDesc => queryable.OrderByDescending(c => c.Name),
            CouponSortingEnum.CreatedTimeAsc => queryable.OrderBy(c => c.CreatedTime),
            CouponSortingEnum.CreatedTimeDesc => queryable.OrderByDescending(c => c.CreatedTime),
            _ => queryable.OrderBy(c => c.Code)
        };

        var paginatedList = await queryable.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
        paginatedList.Meta = new { Count = paginatedList.Data.Count() };
        return paginatedList;
    }
}
