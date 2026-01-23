namespace Application.Features.Brands.Queries.GetBrandPaginatedList;

public record GetBrandPaginatedListResponse(Guid Id, string Name, string? Description, string? ImageUrl, bool IsActive, DateTimeOffset CreatedTime);
