using Application.Common.Bases;

namespace Application.Common.Errors;

public static class UserErrors
{
    public static ApiResponse InvalidCredentials()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Unauthorized,
            Succeeded = false,
            Message = "Invalid email/password"
        };
    }

    public static ApiResponse DisabledUser()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Unauthorized,
            Succeeded = false,
            Message = "Disabled user, please contact your administrator"
        };
    }

    public static ApiResponse LockedUser()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Unauthorized,
            Succeeded = false,
            Message = "Locked user, please contact your administrator"
        };
    }

    public static ApiResponse InvalidJwtToken()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Unauthorized,
            Succeeded = false,
            Message = "Invalid Jwt token"
        };
    }

    public static ApiResponse InvalidRefreshToken()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Unauthorized,
            Succeeded = false,
            Message = "Invalid refresh token"
        };
    }

    public static ApiResponse DuplicatedEmail()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Another user with the same email is already exists"
        };
    }

    public static ApiResponse EmailNotConfirmed()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Unauthorized,
            Succeeded = false,
            Message = "Email is not confirmed"
        };
    }

    public static ApiResponse InvalidCode()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Unauthorized,
            Succeeded = false,
            Message = "Invalid code"
        };
    }

    public static ApiResponse DuplicatedConfirmation()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Email already confirmed"
        };
    }

    public static ApiResponse UserNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "User is not found"
        };
    }

    public static ApiResponse InvalidRoles()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid roles"
        };
    }
}
