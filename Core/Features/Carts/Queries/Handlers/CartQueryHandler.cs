using Core.Features.Carts.Queries.Models;
using Core.Features.Carts.Queries.Responses;

namespace Core.Features.Carts.Queries.Handlers
{
    public class CartQueryHandler : ApiResponseHandler,
        IRequestHandler<GetCartByIdQuery, ApiResponse<GetSingleCartResponse>>,
        IRequestHandler<GetMyCartQuery, ApiResponse<GetSingleCartResponse>>
    {
        #region Fields
        private readonly ICartService _cartService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProductService _productService;
        #endregion

        #region Constructors
        public CartQueryHandler(ICartService cartService,
            ICurrentUserService currentUserService,
            IProductService productService) : base()
        {
            _cartService = cartService;
            _currentUserService = currentUserService;
            _productService = productService;        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<GetSingleCartResponse>> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
        {
            // 1. Get cart from cache
            var cartKey = $"cart:{request.Id}";
            var cart = await _cartService.GetCartByKeyAsync(cartKey);
            if (cart == null) return NotFound<GetSingleCartResponse>(SharedResourcesKeys.CartNotFoundOrEmpty);

            // 2. Extract ProductIds
            var productIds = cart.CartItems.Select(i => i.ProductId).ToList();

            // 3. Query DB to get product names
            var products = await _productService.GetProductsByIdsAsync(productIds);

            // 4. Map response
            var cartMapper = new GetSingleCartResponse
            {
                CreatedAt = cart.CreatedAt,
                CustomerId = cart.CustomerId,
                CartItems = cart.CartItems?.Select(item => new CartItemOfGetSingleResponse
                {
                    ProductId = item.ProductId,
                    ProductName = products.TryGetValue(item.ProductId, out var name) ? name : null,
                    Quantity = item.Quantity,
                    CreatedAt = item.CreatedAt
                }).ToList()
            };

            // 5. Return response
            var resultCart = cartMapper ?? new GetSingleCartResponse { CustomerId = _currentUserService.GetUserId(), CreatedAt = DateTime.UtcNow };
            return Success(resultCart);
        }

        public async Task<ApiResponse<GetSingleCartResponse>> Handle(GetMyCartQuery request, CancellationToken cancellationToken)
        {
            // 1. Get cart from cache
            var cart = await _cartService.GetMyCartAsync();
            if (cart is null) return NotFound<GetSingleCartResponse>(SharedResourcesKeys.CartNotFoundOrEmpty);

            // 2. Extract ProductIds
            var productIds = cart.CartItems.Select(i => i.ProductId).ToList();

            // 3. Query DB to get product names
            var products = await _productService.GetProductsByIdsAsync(productIds);

            // 4. Map response
            var cartMapper = new GetSingleCartResponse
            {
                CreatedAt = cart.CreatedAt,
                CustomerId = cart.CustomerId,
                CartItems = cart.CartItems?.Select(item => new CartItemOfGetSingleResponse
                {
                    ProductId = item.ProductId,
                    ProductName = products.TryGetValue(item.ProductId, out var name) ? name : null,
                    Quantity = item.Quantity,
                    CreatedAt = item.CreatedAt
                }).ToList()
            };

            // 5. Return response
            var resultCart = cartMapper ?? new GetSingleCartResponse { CustomerId = _currentUserService.GetUserId(), CreatedAt = DateTime.UtcNow };
            return Success(resultCart);
        }
        #endregion
    }
}
