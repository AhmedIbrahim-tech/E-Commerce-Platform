using Core.Features.Orders.Queries.Models;
using Core.Features.Orders.Queries.Responses;

namespace Core.Features.Orders.Queries.Handlers
{
    public class OrderQueryHandler : ApiResponseHandler,
        IRequestHandler<GetOrderByIdQuery, ApiResponse<GetSingleOrderResponse>>,
        IRequestHandler<GetMyOrdersQuery, PaginatedResult<GetMyOrdersResponse>>,
        IRequestHandler<GetOrderPaginatedListQuery, PaginatedResult<GetOrderPaginatedListResponse>>
    {
        #region Fields
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        public OrderQueryHandler(
            IOrderService orderService,
            IOrderItemService orderItemService,
            ICurrentUserService currentUserService,
            IMapper mapper) : base()
        {            _orderService = orderService;
            _orderItemService = orderItemService;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<GetSingleOrderResponse>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderByIdAsync(request.Id);
            if (order is null)
                return NotFound<GetSingleOrderResponse>(SharedResourcesKeys.NotFound);
            var orderMapper = _mapper.Map<GetSingleOrderResponse>(order);

            Expression<Func<OrderItem, OrderItemResponse>> expression = orderItem => new OrderItemResponse(
                orderItem.ProductId,
                orderItem.Product != null ? orderItem.Product.Name : null,
                orderItem.Quantity,
                orderItem.UnitPrice
            );
            var orderItemsQueryable = _orderItemService.GetOrderItemsByOrderIdQueryable(request.Id);
            var orderItemPaginatedList = await orderItemsQueryable.Select(expression).ToPaginatedListAsync(request.OrderPageNumber, request.OrderPageSize);
            orderMapper.OrderItems = orderItemPaginatedList;

            return Success(orderMapper);
        }

        public async Task<PaginatedResult<GetOrderPaginatedListResponse>> Handle(GetOrderPaginatedListQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Order, GetOrderPaginatedListResponse>> expression = o => new GetOrderPaginatedListResponse(
            o.Id,
            o.OrderDate,
            o.Status,
            o.TotalAmount,
            o.Customer != null ? o.Customer.FirstName + " " + o.Customer.LastName : null,
            o.ShippingAddress != null ? $"{o.ShippingAddress.Street}, {o.ShippingAddress.City}, {o.ShippingAddress.State}" : null,
            o.Payment!.PaymentMethod,
            o.Payment.PaymentDate,
            o.Payment.Status,
            o.Delivery!.DeliveryMethod,
            o.Delivery.DeliveryTime,
            o.Delivery.Cost,
            o.Delivery.Status);

            var filterQuery = _orderService.FilterOrderPaginatedQueryable(request.SortBy, request.Search!);
            var paginatedList = await filterQuery.Select(expression)
                                                 .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            paginatedList.Meta = new { Count = paginatedList.Data.Count() };
            return paginatedList;
        }

        public async Task<PaginatedResult<GetMyOrdersResponse>> Handle(GetMyOrdersQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            Expression<Func<Order, GetMyOrdersResponse>> expression = o => new GetMyOrdersResponse(
            o.Id,
            o.OrderDate,
            o.Status,
            o.TotalAmount,
            o.Customer != null ? o.Customer.FirstName + " " + o.Customer.LastName : null,
            o.ShippingAddress != null ? $"{o.ShippingAddress.Street}, {o.ShippingAddress.City}, {o.ShippingAddress.State}" : null,
            o.Payment!.PaymentMethod,
            o.Payment.PaymentDate,
            o.Payment.Status,
            o.Delivery!.DeliveryMethod,
            o.Delivery.DeliveryTime,
            o.Delivery.Cost,
            o.Delivery.Status);

            var filterQuery = _orderService.FilterOrderPaginatedByCustomerIdQueryable(request.SortBy, request.Search!, userId);
            var paginatedList = await filterQuery.Select(expression)
                                                 .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            paginatedList.Meta = new { Count = paginatedList.Data.Count() };
            return paginatedList;
        }
        #endregion
    }
}
