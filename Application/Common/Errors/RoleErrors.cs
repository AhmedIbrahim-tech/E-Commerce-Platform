using Application.Common.Bases;

namespace Application.Common.Errors;

public static class RoleErrors
{
    public static ApiResponse RoleNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Role is not found"
        };
    }

    public static ApiResponse DuplicatedRoleName()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Another role with the same name already exists"
        };
    }

    public static ApiResponse InvalidRoleName()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid role name format"
        };
    }

    public static ApiResponse CannotDeleteSystemRole()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Cannot delete system role"
        };
    }

    public static ApiResponse CannotDeleteRoleWithUsers()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Cannot delete role that is assigned to users"
        };
    }

    public static ApiResponse InvalidPermissions()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid permissions specified for this role"
        };
    }

    public static ApiResponse RoleAlreadyAssigned()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Role is already assigned to this user"
        };
    }

    public static ApiResponse RoleNotAssigned()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Role is not assigned to this user"
        };
    }

    public static ApiResponse InsufficientPermissions()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Forbidden,
            Succeeded = false,
            Message = "You do not have sufficient permissions to perform this action"
        };
    }

    public static ApiResponse CannotModifyOwnRole()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Cannot modify your own role"
        };
    }
}

