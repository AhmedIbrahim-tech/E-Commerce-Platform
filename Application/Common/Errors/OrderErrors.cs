using Application.Common.Bases;

namespace Application.Common.Errors;

public static class OrderErrors
{
    public static ApiResponse OrderNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Order is not found"
        };
    }

    public static ApiResponse EmptyCart()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Cannot place order with an empty cart"
        };
    }

    public static ApiResponse InvalidOrderStatus()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid order status for this operation"
        };
    }

    public static ApiResponse OrderAlreadyCancelled()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Order has already been cancelled"
        };
    }

    public static ApiResponse OrderAlreadyCompleted()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Order has already been completed"
        };
    }

    public static ApiResponse CannotCancelOrder()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Order cannot be cancelled at this stage"
        };
    }

    public static ApiResponse InvalidShippingAddress()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid or missing shipping address"
        };
    }

    public static ApiResponse InvalidPaymentMethod()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid payment method selected"
        };
    }

    public static ApiResponse PaymentRequired()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.PaymentRequired,
            Succeeded = false,
            Message = "Payment is required to complete this order"
        };
    }

    public static ApiResponse OrderTotalMismatch()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Order total does not match calculated amount"
        };
    }
}

