namespace Application.Features.Brands.Queries.GetBrandList;

public record GetBrandListResponse(Guid Id, string Name, string? Description, string? ImageUrl, bool IsActive, DateTimeOffset CreatedTime);
