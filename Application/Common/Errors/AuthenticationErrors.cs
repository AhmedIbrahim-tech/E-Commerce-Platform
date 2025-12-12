using Application.Common.Bases;

namespace Application.Common.Errors;

public static class AuthenticationErrors
{
    public static ApiResponse InvalidCredentials()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Unauthorized,
            Succeeded = false,
            Message = "Invalid email or password"
        };
    }

    public static ApiResponse AccountLocked()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Unauthorized,
            Succeeded = false,
            Message = "Account has been locked due to multiple failed login attempts"
        };
    }

    public static ApiResponse EmailNotVerified()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Unauthorized,
            Succeeded = false,
            Message = "Email address has not been verified"
        };
    }

    public static ApiResponse InvalidResetToken()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid or expired password reset token"
        };
    }

    public static ApiResponse ResetTokenExpired()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Password reset token has expired"
        };
    }

    public static ApiResponse InvalidVerificationCode()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid or expired verification code"
        };
    }

    public static ApiResponse VerificationCodeExpired()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Verification code has expired"
        };
    }

    public static ApiResponse TooManyLoginAttempts()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.TooManyRequests,
            Succeeded = false,
            Message = "Too many login attempts. Please try again later"
        };
    }

    public static ApiResponse SocialLoginFailed()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Social login authentication failed"
        };
    }

    public static ApiResponse SessionExpired()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Unauthorized,
            Succeeded = false,
            Message = "Your session has expired. Please login again"
        };
    }
}

