using Application.Common.Bases;

namespace Application.Common.Errors;

public static class ShippingAddressErrors
{
    public static ApiResponse ShippingAddressNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Shipping address is not found"
        };
    }

    public static ApiResponse InvalidAddress()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid shipping address provided"
        };
    }

    public static ApiResponse MissingRequiredFields()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Required shipping address fields are missing"
        };
    }

    public static ApiResponse InvalidPostalCode()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid postal code format"
        };
    }

    public static ApiResponse InvalidPhoneNumber()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid phone number format for shipping address"
        };
    }

    public static ApiResponse DuplicatedAddress()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "This shipping address already exists"
        };
    }

    public static ApiResponse CannotDeleteDefaultAddress()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Cannot delete default shipping address"
        };
    }

    public static ApiResponse CannotDeleteAddressWithOrders()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Cannot delete shipping address that has been used in orders"
        };
    }

    public static ApiResponse MaximumAddressesExceeded()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Maximum number of shipping addresses has been exceeded"
        };
    }

    public static ApiResponse UnsupportedShippingRegion()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Shipping is not available to this region"
        };
    }
}

