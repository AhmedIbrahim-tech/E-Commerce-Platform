using Application.Common.Bases;

namespace Application.Common.Errors;

public static class CartErrors
{
    public static ApiResponse CartNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Cart is not found"
        };
    }

    public static ApiResponse CartItemNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Cart item is not found"
        };
    }

    public static ApiResponse ProductAlreadyInCart()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Product is already in the cart"
        };
    }

    public static ApiResponse InvalidQuantity()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid quantity specified. Quantity must be greater than zero"
        };
    }

    public static ApiResponse QuantityExceedsStock()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Requested quantity exceeds available stock"
        };
    }

    public static ApiResponse EmptyCart()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Cart is empty"
        };
    }

    public static ApiResponse CartExpired()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Cart has expired and has been cleared"
        };
    }

    public static ApiResponse CannotModifyCart()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Cannot modify cart at this time"
        };
    }

    public static ApiResponse MaximumCartItemsExceeded()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Maximum number of cart items has been exceeded"
        };
    }

    public static ApiResponse InvalidCartOperation()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid cart operation"
        };
    }
}

