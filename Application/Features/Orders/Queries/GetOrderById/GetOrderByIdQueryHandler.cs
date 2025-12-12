using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetOrderByIdQuery, ApiResponse<GetOrderByIdResponse>>
{
    public async Task<ApiResponse<GetOrderByIdResponse>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.Orders.GetTableNoTracking()
            .Where(c => c.Id.Equals(request.Id))
            .Include(c => c.Customer)
            .Include(c => c.ShippingAddress)
            .Include(c => c.Payment)
            .Include(c => c.Delivery)
            .FirstOrDefaultAsync(cancellationToken);
        if (order is null)
            return new ApiResponse<GetOrderByIdResponse>(OrderErrors.OrderNotFound());

        var orderResponse = new GetOrderByIdResponse
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            OrderStatus = order.Status,
            TotalAmount = order.TotalAmount,
            CustomerName = order.Customer != null ? order.Customer.FullName : null,
            ShippingAddress = order.ShippingAddress != null ? $"{order.ShippingAddress.Street}, {order.ShippingAddress.City}, {order.ShippingAddress.State}" : null,
            PaymentMethod = order.Payment?.PaymentMethod,
            PaymentDate = order.Payment?.PaymentDate,
            PaymentStatus = order.Payment?.Status,
            DeliveryMethod = order.Delivery?.DeliveryMethod,
            DeliveryTime = order.Delivery?.DeliveryTime,
            DeliveryCost = order.Delivery?.Cost
        };

        Expression<Func<OrderItem, OrderItemResponse>> expression = orderItem => new OrderItemResponse(
            orderItem.ProductId,
            orderItem.Product != null ? orderItem.Product.Name : null,
            orderItem.Quantity,
            orderItem.UnitPrice
        );

        var orderItemsQueryable = unitOfWork.OrderItems.GetTableNoTracking()
            .Where(r => r.OrderId.Equals(request.Id))
            .Include(r => r.Product)
            .AsQueryable();

        var orderItemPaginatedList = await orderItemsQueryable.Select(expression).ToPaginatedListAsync(request.OrderPageNumber, request.OrderPageSize);
        orderResponse.OrderItems = orderItemPaginatedList;

        return Success(orderResponse);
    }
}

