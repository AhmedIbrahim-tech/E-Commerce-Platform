namespace Application.Features.SubCategories.Queries.GetSubCategoryPaginatedList;

public record GetSubCategoryPaginatedListResponse(Guid Id, string Name, string? Description, string? ImageUrl, string? Code, Guid CategoryId, string? CategoryName, bool IsActive, DateTimeOffset CreatedTime);
