using Application.Common.Bases;
using Application.Common.Errors;
using Domain.Entities.Cart;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Application.Features.Carts.Queries.GetCartById;

public class GetCartByIdQueryHandler(
    IMemoryCache memoryCache,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetCartByIdQuery, ApiResponse<GetCartByIdResponse>>
{
    public async Task<ApiResponse<GetCartByIdResponse>> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
    {
        // 1. Get cart from cache
        var cartKey = $"cart:{request.Id}";
        var cart = GetCartByKey(cartKey);
        if (cart == null) return new ApiResponse<GetCartByIdResponse>(CartErrors.CartNotFound());

        // 2. Extract ProductIds
        var productIds = cart.CartItems?.Select(i => i.ProductId).ToList() ?? new List<Guid>();

        // 3. Query DB to get product names
        var products = await unitOfWork.Products.GetProductsByIdsAsync(productIds, cancellationToken);

        // 4. Map response using Select()
        var cartResponse = new GetCartByIdResponse
        {
            CreatedTime = cart.CreatedTime,
            CustomerId = cart.CustomerId,
            CartItems = cart.CartItems?.Select(item => new CartItemResponse
            {
                ProductId = item.ProductId,
                ProductName = products.TryGetValue(item.ProductId, out var name) ? name : null,
                Quantity = item.Quantity,
                CreatedTime = item.CreatedTime
            }).ToList()
        };

        // 5. Return response
        var resultCart = cartResponse ?? new GetCartByIdResponse { CustomerId = currentUserService.GetUserId(), CreatedTime = DateTimeOffset.UtcNow };
        return Success(resultCart);
    }

    private Cart? GetCartByKey(string cartKey)
    {
        if (!memoryCache.TryGetValue(cartKey, out string? cached))
            return null;
        return JsonSerializer.Deserialize<Cart>(cached ?? "");
    }


}

