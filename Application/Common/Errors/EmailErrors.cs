using Application.Common.Bases;

namespace Application.Common.Errors;

public static class EmailErrors
{
    public static ApiResponse EmailSendFailed()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.InternalServerError,
            Succeeded = false,
            Message = "Failed to send email"
        };
    }

    public static ApiResponse InvalidEmailAddress()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid email address format"
        };
    }

    public static ApiResponse InvalidEmailType()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid email type specified"
        };
    }

    public static ApiResponse EmailTemplateNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Email template is not found"
        };
    }

    public static ApiResponse EmptyEmailContent()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Email content cannot be empty"
        };
    }

    public static ApiResponse InvalidEmailRecipient()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid email recipient specified"
        };
    }

    public static ApiResponse EmailRateLimitExceeded()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.TooManyRequests,
            Succeeded = false,
            Message = "Email rate limit has been exceeded. Please try again later"
        };
    }

    public static ApiResponse EmailServiceUnavailable()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.ServiceUnavailable,
            Succeeded = false,
            Message = "Email service is currently unavailable"
        };
    }

    public static ApiResponse EmailAlreadySent()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Email has already been sent"
        };
    }

    public static ApiResponse InvalidEmailConfiguration()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.InternalServerError,
            Succeeded = false,
            Message = "Email service configuration is invalid"
        };
    }
}

