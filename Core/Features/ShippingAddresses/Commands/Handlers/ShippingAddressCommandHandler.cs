using Core.Features.ShippingAddresses.Commands.Models;

namespace Core.Features.ShippingAddresses.Commands.Handlers
{
    public class ShippingAddressCommandHandler : ApiResponseHandler,
        IRequestHandler<AddShippingAddressCommand, ApiResponse<string>>,
        IRequestHandler<SetShippingAddressCommand, ApiResponse<string>>,
        IRequestHandler<EditShippingAddressCommand, ApiResponse<string>>,
        IRequestHandler<DeleteShippingAddressCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly IShippingAddressService _shippingAddressService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        #endregion

        #region Constructors
        public ShippingAddressCommandHandler(
            IShippingAddressService shippingAddressService,
            IOrderService orderService,
            IMapper mapper,
            ICurrentUserService currentUserService) : base()
        {
            _shippingAddressService = shippingAddressService;
            _orderService = orderService;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(AddShippingAddressCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.GetUserId();
            var shippingAddressMapper = _mapper.Map<ShippingAddress>(request);
            shippingAddressMapper.CustomerId = currentUserId;
            var result = await _shippingAddressService.AddShippingAddressAsync(shippingAddressMapper);
            if (result == "Success") return Created("");
            return BadRequest<string>("CreateFailed");
        }

        public async Task<ApiResponse<string>> Handle(SetShippingAddressCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderByIdAsync(request.OrderId);
            if (order == null || order.Status != Status.Draft)
                return BadRequest<string>("InvalidOrder");

            var shippingAddress = await _shippingAddressService.GetShippingAddressByIdAsync(request.ShippingAddressId);
            if (shippingAddress == null)
                return BadRequest<string>("ShippingAddressDoesNotExist");

            var deliveryOffset = DeliveryTimeCalculator.Calculate(shippingAddress.City, order.Delivery!.DeliveryMethod);
            var deliveryCost = DeliveryCostCalculator.Calculate(shippingAddress.City, order.Delivery.DeliveryMethod);

            order.ShippingAddressId = request.ShippingAddressId;
            order.Delivery.Description = $"Delivery for order #{order.Id} to {shippingAddress.State}, {shippingAddress.City}, {shippingAddress.Street}";
            order.Delivery.DeliveryTime = DateTime.UtcNow.Add(deliveryOffset);
            order.Delivery.Cost = deliveryCost;

            var result = await _orderService.EditOrderAsync(order);
            if (result != "Success")
                return BadRequest<string>("UpdateFailed");
            return Success("");
        }

        public async Task<ApiResponse<string>> Handle(EditShippingAddressCommand request, CancellationToken cancellationToken)
        {
            var shippingAddress = await _shippingAddressService.GetShippingAddressByIdAsync(request.Id);
            if (shippingAddress == null) return NotFound<string>("ShippingAddressDoesNotExist");
            var shippingAddressMapper = _mapper.Map<ShippingAddress>(request);
            shippingAddressMapper.CustomerId = shippingAddress.CustomerId;
            var result = await _shippingAddressService.EditShippingAddressAsync(shippingAddressMapper);
            if (result == "Success") return Edit("");
            return BadRequest<string>("UpdateFailed");
        }

        public async Task<ApiResponse<string>> Handle(DeleteShippingAddressCommand request, CancellationToken cancellationToken)
        {
            var shippingAddress = await _shippingAddressService.GetShippingAddressByIdAsync(request.Id);
            if (shippingAddress == null) return NotFound<string>("ShippingAddressDoesNotExist");
            var result = await _shippingAddressService.DeleteShippingAddressAsync(shippingAddress);
            if (result == "Success") return Deleted<string>();
            return BadRequest<string>("DeleteFailed");
        }
        #endregion
    }
}
