namespace Application.Features.SubCategories.Queries.GetSubCategoryById;

public record GetSubCategoryByIdResponse(Guid Id, string Name, string? Description, string? ImageUrl, string? Code, Guid CategoryId, string? CategoryName, bool IsActive, DateTimeOffset CreatedTime);
