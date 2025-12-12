using Microsoft.AspNetCore.Mvc;

namespace Application.Common.Bases;

public static class ResultExtensions
{
    public static ObjectResult ToProblem<T>(this ApiResponse<T> response)
    {
        if (response.Succeeded)
            throw new InvalidOperationException("Cannot convert success result to a problem");

        var statusCode = (int)response.StatusCode;
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = "An error occurred",
            Detail = response.Message
        };

        var errors = new List<string>();
        if (!string.IsNullOrEmpty(response.Message))
            errors.Add(response.Message);
        
        if (response.Errors != null && response.Errors.Any())
            errors.AddRange(response.Errors);

        problemDetails.Extensions = new Dictionary<string, object?>
        {
            {
                "errors", errors
            }
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode
        };
    }
}

