using Core.Features.Carts.Commands.Models;

namespace Core.Features.Carts.Commands.Handlers
{
    public class CartCommandHandler : ApiResponseHandler,
        IRequestHandler<AddToCartCommand, ApiResponse<string>>,
        IRequestHandler<RemoveFromCartCommand, ApiResponse<string>>,
        IRequestHandler<UpdateItemQuantityCommand, ApiResponse<string>>,
        IRequestHandler<DeleteCartCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly ICartService _cartService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProductService _productService;
        #endregion

        #region Constructors
        public CartCommandHandler(ICartService cartService,
            ICurrentUserService currentUserService,
            IProductService productService) : base()
        {
            _cartService = cartService;
            _currentUserService = currentUserService;
            _productService = productService;        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
        {
            var result = await _cartService.DeleteCartAsync(request.CartId);
            if (result) return Deleted<string>();
            return BadRequest<string>(SharedResourcesKeys.DeleteFailed);
        }

        public async Task<ApiResponse<string>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var result = await _cartService.AddToCartAsync(request.ProductId, request.Quantity);
            return result switch
            {
                "Success" => Success<string>(SharedResourcesKeys.AddedToCart),
                "ProductNotFound" => NotFound<string>(SharedResourcesKeys.ProductNotFound),
                "FailedInAddItemToCart" => BadRequest<string>(SharedResourcesKeys.FailedToModifyThisCart),
                "ItemAlreadyExistsInCart" => BadRequest<string>(SharedResourcesKeys.ItemAlreadyExistsInCart),
                _ => BadRequest<string>(SharedResourcesKeys.AnErrorOccurredWhileAddingToTheCart)
            };
        }

        public async Task<ApiResponse<string>> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
        {
            var result = await _cartService.RemoveItemFromCartAsync(request.ProductId);
            return result switch
            {
                "Success" => Success<string>(SharedResourcesKeys.ItemRemovedFromCart),
                "CartNotFound" => NotFound<string>(SharedResourcesKeys.CartNotFoundOrEmpty),
                "ProductNotFound" => NotFound<string>(SharedResourcesKeys.ProductNotFound),
                "ItemNotFoundInCart" => NotFound<string>(SharedResourcesKeys.ItemNotFoundInCart),
                "FailedInRemoveItemFromCart" => BadRequest<string>(SharedResourcesKeys.FailedToModifyThisCart),
                _ => BadRequest<string>(SharedResourcesKeys.AnErrorOccurredWhileRemovingFromTheCart)
            };
        }

        public async Task<ApiResponse<string>> Handle(UpdateItemQuantityCommand request, CancellationToken cancellationToken)
        {
            var result = await _cartService.UpdateItemQuantityAsync(request.ProductId, request.Quantity);
            return result switch
            {
                "Success" => Success<string>(SharedResourcesKeys.ItemQuantityUpdated),
                "CartNotFound" => NotFound<string>(SharedResourcesKeys.CartNotFoundOrEmpty),
                "ProductNotFound" => NotFound<string>(SharedResourcesKeys.ProductNotFound),
                "ItemNotFoundInCart" => NotFound<string>(SharedResourcesKeys.ItemNotFoundInCart),
                "FailedInUpdateItemQuantity" => BadRequest<string>(SharedResourcesKeys.FailedToModifyThisCart),
                _ => BadRequest<string>(SharedResourcesKeys.AnErrorOccurredWhileUpdatingItemQuantity)
            };
        }
        #endregion
    }
}
