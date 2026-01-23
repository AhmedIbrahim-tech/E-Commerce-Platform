using Application.Common.Bases;

namespace Application.Common.Errors;

public static class AuditLogErrors
{
    public static ApiResponse AuditLogNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Audit log is not found"
        };
    }
}
