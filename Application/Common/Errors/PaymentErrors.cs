using Application.Common.Bases;

namespace Application.Common.Errors;

public static class PaymentErrors
{
    public static ApiResponse PaymentNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Payment is not found"
        };
    }

    public static ApiResponse PaymentFailed()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Payment processing failed"
        };
    }

    public static ApiResponse InvalidPaymentMethod()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid payment method specified"
        };
    }

    public static ApiResponse PaymentAlreadyProcessed()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Payment has already been processed"
        };
    }

    public static ApiResponse PaymentAmountMismatch()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Payment amount does not match order total"
        };
    }

    public static ApiResponse InsufficientFunds()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Insufficient funds to complete the payment"
        };
    }

    public static ApiResponse PaymentDeclined()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Payment was declined by the payment provider"
        };
    }

    public static ApiResponse PaymentExpired()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Payment session has expired"
        };
    }

    public static ApiResponse InvalidPaymentToken()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid or expired payment token"
        };
    }

    public static ApiResponse PaymentProviderError()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.InternalServerError,
            Succeeded = false,
            Message = "An error occurred while processing payment with the payment provider"
        };
    }
}

