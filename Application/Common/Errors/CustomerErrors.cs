using Application.Common.Bases;

namespace Application.Common.Errors;

public static class CustomerErrors
{
    public static ApiResponse CustomerNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Customer is not found"
        };
    }

    public static ApiResponse DuplicatedPhoneNumber()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Another customer with the same phone number already exists"
        };
    }

    public static ApiResponse InvalidPhoneNumber()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid phone number format"
        };
    }

    public static ApiResponse CustomerAlreadyExists()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Customer with this information already exists"
        };
    }

    public static ApiResponse CustomerInactive()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Customer account is inactive"
        };
    }

    public static ApiResponse CannotDeleteCustomerWithOrders()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Cannot delete customer that has placed orders"
        };
    }

    public static ApiResponse InvalidCustomerData()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid customer data provided"
        };
    }

    public static ApiResponse CustomerProfileIncomplete()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Customer profile information is incomplete"
        };
    }

    public static ApiResponse CannotModifyCustomerStatus()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Cannot modify customer status at this time"
        };
    }

    public static ApiResponse CustomerAccountSuspended()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Forbidden,
            Succeeded = false,
            Message = "Customer account has been suspended"
        };
    }
}

