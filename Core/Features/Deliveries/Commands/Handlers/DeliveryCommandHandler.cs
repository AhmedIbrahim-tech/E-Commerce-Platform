using Core.Features.Deliveries.Commands.Models;

namespace Core.Features.Deliveries.Commands.Handlers
{
    public class DeliveryCommandHandler : ApiResponseHandler,
        IRequestHandler<SetDeliveryMethodCommand, ApiResponse<string>>,
        IRequestHandler<EditDeliveryMethodCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly IOrderService _orderService;
        #endregion

        #region Constructors
        public DeliveryCommandHandler(
            IOrderService orderService) : base()
        {            _orderService = orderService;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(SetDeliveryMethodCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderByIdAsync(request.OrderId);
            if (order == null || order.Status != Status.Draft)
                return BadRequest<string>(SharedResourcesKeys.InvalidOrder);

            if (order.Delivery == null)
                order.Delivery = new Delivery();

            order.Delivery.DeliveryMethod = request.DeliveryMethod;
            order.Delivery.Status = Status.Draft;

            var result = await _orderService.EditOrderAsync(order);
            if (result != "Success")
                return BadRequest<string>(SharedResourcesKeys.UpdateFailed);
            return Success("");
        }

        public async Task<ApiResponse<string>> Handle(EditDeliveryMethodCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderByIdAsync(request.OrderId);
            if (order == null || order.Status != Status.Draft)
                return BadRequest<string>(SharedResourcesKeys.InvalidOrder);

            if (order.Delivery == null)
                return NotFound<string>(SharedResourcesKeys.NotFound);

            order.Delivery.DeliveryMethod = request.DeliveryMethod;
            order.Delivery.Status = Status.Draft;

            var result = await _orderService.EditOrderAsync(order);
            if (result != "Success")
                return BadRequest<string>(SharedResourcesKeys.UpdateFailed);
            return Success("");
        }
        #endregion
    }
}
