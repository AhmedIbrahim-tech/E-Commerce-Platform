using Application.Common.Bases;

namespace Application.Common.Errors;

public static class VendorErrors
{
    public static ApiResponse VendorNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Vendor is not found"
        };
    }

    public static ApiResponse DuplicatedStoreName()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Another vendor with the same store name already exists"
        };
    }

    public static ApiResponse InvalidCommissionRate()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid commission rate. Must be between 0 and 100"
        };
    }

    public static ApiResponse VendorAlreadyExists()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Vendor with this information already exists"
        };
    }

    public static ApiResponse VendorInactive()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Vendor account is inactive"
        };
    }

    public static ApiResponse CannotDeleteVendorWithProducts()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Cannot delete vendor that has products"
        };
    }

    public static ApiResponse InvalidVendorData()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid vendor data provided"
        };
    }

    public static ApiResponse VendorProfileIncomplete()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Vendor profile information is incomplete"
        };
    }
}
