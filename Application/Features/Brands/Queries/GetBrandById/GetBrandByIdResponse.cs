namespace Application.Features.Brands.Queries.GetBrandById;

public record GetBrandByIdResponse(Guid Id, string Name, string? Description, string? ImageUrl, bool IsActive, DateTimeOffset CreatedTime);
