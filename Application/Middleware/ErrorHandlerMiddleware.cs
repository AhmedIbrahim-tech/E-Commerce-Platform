using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.Middleware;

public class ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception error)
        {
            await HandleExceptionAsync(context, error, logger);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception error, ILogger<ErrorHandlerMiddleware> logger)
    {
        var (statusCode, logLevel) = GetStatusCodeAndLogLevel(error);
        
        LogException(error, statusCode, logLevel, logger);

        var problemDetails = new ProblemDetails
        {
            Title = GetTitle(error),
            Detail = GetDetail(error),
            Status = (int)statusCode,
            Instance = context.Request.Path,
            Extensions =
            {
                ["traceId"] = context.TraceIdentifier
            }
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private static (HttpStatusCode StatusCode, LogLevel LogLevel) GetStatusCodeAndLogLevel(Exception error)
    {
        return error switch
        {
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, LogLevel.Warning),
            KeyNotFoundException => (HttpStatusCode.NotFound, LogLevel.Information),
            DbUpdateException => (HttpStatusCode.BadRequest, LogLevel.Warning),
            _ when IsValidationException(error) => (HttpStatusCode.UnprocessableEntity, LogLevel.Warning),
            _ => (HttpStatusCode.InternalServerError, LogLevel.Error)
        };
    }

    private static string GetTitle(Exception error)
    {
        return error switch
        {
            UnauthorizedAccessException => "Unauthorized",
            KeyNotFoundException => "Not Found",
            DbUpdateException => "Database Error",
            _ when IsValidationException(error) => "Validation Error",
            _ => "An error occurred while processing your request"
        };
    }

    private static bool IsValidationException(Exception error)
    {
        return error.GetType().Name.Contains("ValidationException", StringComparison.OrdinalIgnoreCase);
    }

    private static string GetDetail(Exception error)
    {
        return error.InnerException != null
            ? $"{error.Message} {error.InnerException.Message}"
            : error.Message;
    }

    private static void LogException(Exception error, HttpStatusCode statusCode, LogLevel logLevel, ILogger<ErrorHandlerMiddleware> logger)
    {
        var logMessage = "Exception occurred: {ExceptionType} | Status: {StatusCode} | Message: {Message}";
        var args = new object[] { error.GetType().Name, (int)statusCode, error.Message };

        switch (logLevel)
        {
            case LogLevel.Critical:
                logger.LogCritical(error, logMessage, args);
                break;
            case LogLevel.Error:
                logger.LogError(error, logMessage, args);
                break;
            case LogLevel.Warning:
                logger.LogWarning(error, logMessage, args);
                break;
            case LogLevel.Information:
                logger.LogInformation(error, logMessage, args);
                break;
            default:
                logger.LogError(error, logMessage, args);
                break;
        }
    }
}
