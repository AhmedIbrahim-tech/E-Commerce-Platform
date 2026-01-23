using Application.Common.Bases;

namespace Application.Common.Errors;

public static class AdminErrors
{
    public static ApiResponse AdminNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Admin is not found"
        };
    }

    public static ApiResponse DuplicatedEmail()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Another admin with the same email already exists"
        };
    }

    public static ApiResponse AdminAlreadyExists()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Admin with this information already exists"
        };
    }

    public static ApiResponse AdminInactive()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Admin account is inactive"
        };
    }

    public static ApiResponse CannotDeleteAdmin()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Cannot delete admin account"
        };
    }

    public static ApiResponse InvalidAdminData()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid admin data provided"
        };
    }

    public static ApiResponse AdminProfileIncomplete()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Admin profile information is incomplete"
        };
    }
}
