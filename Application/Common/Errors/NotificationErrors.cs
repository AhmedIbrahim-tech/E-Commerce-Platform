using Application.Common.Bases;

namespace Application.Common.Errors;

public static class NotificationErrors
{
    public static ApiResponse NotificationNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Notification is not found"
        };
    }

    public static ApiResponse InvalidNotificationType()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid notification type specified"
        };
    }

    public static ApiResponse InvalidRecipient()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid notification recipient specified"
        };
    }

    public static ApiResponse NotificationSendFailed()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.InternalServerError,
            Succeeded = false,
            Message = "Failed to send notification"
        };
    }

    public static ApiResponse EmptyNotificationContent()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Notification content cannot be empty"
        };
    }

    public static ApiResponse NotificationAlreadyRead()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Notification has already been marked as read"
        };
    }

    public static ApiResponse InvalidNotificationStatus()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid notification status"
        };
    }

    public static ApiResponse CannotDeleteNotification()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Cannot delete this notification"
        };
    }

    public static ApiResponse NotificationTemplateNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Notification template is not found"
        };
    }

    public static ApiResponse NotificationRateLimitExceeded()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.TooManyRequests,
            Succeeded = false,
            Message = "Notification rate limit has been exceeded"
        };
    }
}

