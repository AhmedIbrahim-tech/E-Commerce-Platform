using Application.Common.Bases;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Orders.Queries.GetMyOrders;

public class GetMyOrdersQueryHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<GetMyOrdersQuery, PaginatedResult<GetMyOrdersResponse>>
{
    public async Task<PaginatedResult<GetMyOrdersResponse>> Handle(GetMyOrdersQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserId();
        Expression<Func<Order, GetMyOrdersResponse>> expression = o => new GetMyOrdersResponse(
            o.Id,
            o.OrderDate,
            o.Status,
            o.TotalAmount,
            o.Customer != null ? o.Customer.FullName : null,
            o.ShippingAddress != null ? $"{o.ShippingAddress.Street}, {o.ShippingAddress.City}, {o.ShippingAddress.State}" : null,
            o.Payment!.PaymentMethod,
            o.Payment.PaymentDate,
            o.Payment.Status,
            o.Delivery!.DeliveryMethod,
            o.Delivery.DeliveryTime,
            o.Delivery.Cost,
            o.Delivery.Status);

        var queryable = unitOfWork.Orders.GetTableNoTracking()
            .Where(o => o.CustomerId == userId)
            .Include(c => c.Customer)
            .Include(c => c.ShippingAddress)
            .Include(c => c.Payment)
            .Include(c => c.Delivery)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
            queryable = queryable.Where(c => (c.Customer!.FullName != null && c.Customer.FullName.Contains(request.Search!))
                                          || c.ShippingAddress!.City!.Contains(request.Search!)
                                          || c.ShippingAddress!.Street!.Contains(request.Search!)
                                          || c.ShippingAddress!.State!.Contains(request.Search!));

        queryable = request.SortBy switch
        {
            OrderSortingEnum.OrderDateAsc => queryable.OrderBy(c => c.OrderDate),
            OrderSortingEnum.OrderDateDesc => queryable.OrderByDescending(c => c.OrderDate),
            OrderSortingEnum.TotalAmountAsc => queryable.OrderBy(c => c.TotalAmount),
            OrderSortingEnum.TotalAmountDesc => queryable.OrderByDescending(c => c.TotalAmount),
            _ => queryable.OrderByDescending(c => c.OrderDate)
        };

        var paginatedList = await queryable.Select(expression)
                                             .ToPaginatedListAsync(request.PageNumber, request.PageSize);
        paginatedList.Meta = new { Count = paginatedList.Data.Count() };
        return paginatedList;
    }
}

