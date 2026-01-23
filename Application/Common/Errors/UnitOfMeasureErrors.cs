using Application.Common.Bases;

namespace Application.Common.Errors;

public static class UnitOfMeasureErrors
{
    public static ApiResponse UnitOfMeasureNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Unit of measure is not found"
        };
    }

    public static ApiResponse DuplicatedUnitOfMeasureName()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Another unit of measure with the same name already exists"
        };
    }

    public static ApiResponse DuplicatedUnitOfMeasureShortName()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Another unit of measure with the same short name already exists"
        };
    }

    public static ApiResponse CannotDeleteUnitOfMeasureWithProducts()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Cannot delete unit of measure that contains products"
        };
    }
}
