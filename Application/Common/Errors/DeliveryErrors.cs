using Application.Common.Bases;

namespace Application.Common.Errors;

public static class DeliveryErrors
{
    public static ApiResponse DeliveryNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Delivery is not found"
        };
    }

    public static ApiResponse InvalidDeliveryMethod()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid delivery method specified"
        };
    }

    public static ApiResponse DeliveryMethodNotAvailable()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Selected delivery method is not available for this location"
        };
    }

    public static ApiResponse InvalidDeliveryDate()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid delivery date specified"
        };
    }

    public static ApiResponse DeliveryDateInPast()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Delivery date cannot be in the past"
        };
    }

    public static ApiResponse CannotModifyDeliveryStatus()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Cannot modify delivery status at this stage"
        };
    }

    public static ApiResponse DeliveryAlreadyCompleted()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Delivery has already been completed"
        };
    }

    public static ApiResponse DeliveryAlreadyCancelled()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Delivery has already been cancelled"
        };
    }

    public static ApiResponse InvalidTrackingNumber()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid tracking number format"
        };
    }

    public static ApiResponse DeliveryCostCalculationFailed()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.InternalServerError,
            Succeeded = false,
            Message = "Failed to calculate delivery cost"
        };
    }
}

