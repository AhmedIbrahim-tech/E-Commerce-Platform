using Application.Common.Bases;

namespace Application.Common.Errors;

public static class EmployeeErrors
{
    public static ApiResponse EmployeeNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Employee is not found"
        };
    }

    public static ApiResponse DuplicatedEmployeeEmail()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Another employee with the same email already exists"
        };
    }

    public static ApiResponse DuplicatedEmployeeId()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Another employee with the same employee ID already exists"
        };
    }

    public static ApiResponse InvalidEmployeeData()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid employee data provided"
        };
    }

    public static ApiResponse EmployeeInactive()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Employee account is inactive"
        };
    }

    public static ApiResponse CannotDeleteEmployeeWithOrders()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Cannot delete employee that has processed orders"
        };
    }

    public static ApiResponse InvalidDepartment()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid department specified"
        };
    }

    public static ApiResponse InvalidPosition()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid position specified"
        };
    }

    public static ApiResponse EmployeeAlreadyAssigned()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Employee is already assigned to this department"
        };
    }

    public static ApiResponse CannotModifyEmployeeStatus()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Cannot modify employee status at this time"
        };
    }
}

