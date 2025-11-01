using System.Text.Json;

namespace Core.Middleware;

public class ErrorHandlerMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            // 🔍 Determine if it's a warning or error
            bool isWarning = IsWarning(error);

            // 🧾 Build readable message and description
            string title = isWarning ? "⚠️ Warning Detected" : "❌ Error Occurred";
            string message = GetReadableMessage(error, isWarning);
            string description = GetDetailedDescription(error);

            // 🧱 Prepare API response
            var statusCode = DetermineStatusCode(error);
            var responseModel = new ApiResponse<object>
            {
                Succeeded = false,
                Message = message,
                Data = new
                {
                    Type = isWarning ? "Warning" : "Error",
                    Description = description
                },
                StatusCode = statusCode
            };

            response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await response.WriteAsync(json);
        }
    }

    private static bool IsWarning(Exception error)
    {
        return error.Message.Contains("warning", StringComparison.OrdinalIgnoreCase)
            || error.GetType().Name.Contains("Warning", StringComparison.OrdinalIgnoreCase)
            || error.Message.Contains("PendingModelChangesWarning", StringComparison.OrdinalIgnoreCase);
    }

    private string GetReadableMessage(Exception error, bool isWarning)
    {
        if (isWarning)
            return "A warning was detected while processing your request.";

        if (error is DbUpdateException)
            return "A database error occurred. Please try again later.";

        if (error is ValidationException)
            return "Validation failed for one or more inputs.";

        return error.Message;
    }

    private static string GetDetailedDescription(Exception error)
    {
        return error.InnerException != null
            ? $"{error.Message} | Inner: {error.InnerException.Message}"
            : error.Message;
    }

    private static HttpStatusCode DetermineStatusCode(Exception error)
    {
        return error switch
        {
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            ValidationException => HttpStatusCode.UnprocessableEntity,
            KeyNotFoundException => HttpStatusCode.NotFound,
            DbUpdateException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };
    }
}
