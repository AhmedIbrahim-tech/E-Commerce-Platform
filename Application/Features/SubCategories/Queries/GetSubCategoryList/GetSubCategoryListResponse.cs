namespace Application.Features.SubCategories.Queries.GetSubCategoryList;

public record GetSubCategoryListResponse(Guid Id, string Name, string? Description, string? ImageUrl, string? Code, Guid CategoryId, string? CategoryName, bool IsActive, DateTimeOffset CreatedTime);
