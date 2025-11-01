using Core.Features.ShippingAddresses.Queries.Models;
using Core.Features.ShippingAddresses.Queries.Responses;

namespace Core.Features.ShippingAddresses.Queries.Handlers
{
    public class ShippingAddressQueryHandler : ApiResponseHandler,
        IRequestHandler<GetShippingAddressListQuery, ApiResponse<List<GetShippingAddressListResponse>>>,
        IRequestHandler<GetSingleShippingAddressQuery, ApiResponse<GetSingleShippingAddressResponse>>
    {
        #region Fields
        private readonly IShippingAddressService _shippingAddressService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        #endregion

        #region Constructors
        public ShippingAddressQueryHandler(
            IShippingAddressService shippingAddressService,
            IMapper mapper,
            ICurrentUserService currentUserService) : base()
        {
            _shippingAddressService = shippingAddressService;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<List<GetShippingAddressListResponse>>> Handle(GetShippingAddressListQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.GetUserId();
            var shippingAddressList = await _shippingAddressService.GetShippingAddressListByCustomerIdAsync(currentUserId);
            var shippingAddressListMapper = _mapper.Map<List<GetShippingAddressListResponse>>(shippingAddressList);
            return Success(shippingAddressListMapper);
        }

        public async Task<ApiResponse<GetSingleShippingAddressResponse>> Handle(GetSingleShippingAddressQuery request, CancellationToken cancellationToken)
        {
            var shippingAddress = await _shippingAddressService.GetShippingAddressByIdAsync(request.Id);
            if (shippingAddress is null) return NotFound<GetSingleShippingAddressResponse>(SharedResourcesKeys.ShippingAddressDoesNotExist);
            var shippingAddressMapper = _mapper.Map<GetSingleShippingAddressResponse>(shippingAddress);
            return Success(shippingAddressMapper);
        }
        #endregion
    }
}
