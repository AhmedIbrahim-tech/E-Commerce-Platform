using Application.Common.Bases;

namespace Application.Common.Errors;

public static class PermissionErrors
{
    public static ApiResponse PermissionNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Permission is not found"
        };
    }

    public static ApiResponse DuplicatedPermission()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Permission with the same name already exists"
        };
    }

    public static ApiResponse InvalidPermissionName()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid permission name format"
        };
    }

    public static ApiResponse PermissionDenied()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Forbidden,
            Succeeded = false,
            Message = "You do not have permission to perform this action"
        };
    }

    public static ApiResponse CannotDeleteSystemPermission()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Cannot delete system permission"
        };
    }

    public static ApiResponse PermissionAlreadyAssigned()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Permission is already assigned"
        };
    }

    public static ApiResponse PermissionNotAssigned()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Permission is not assigned"
        };
    }

    public static ApiResponse InvalidPermissionScope()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid permission scope specified"
        };
    }

    public static ApiResponse RequiredPermissionMissing()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Forbidden,
            Succeeded = false,
            Message = "Required permission is missing"
        };
    }

    public static ApiResponse CannotModifyOwnPermissions()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Cannot modify your own permissions"
        };
    }
}

