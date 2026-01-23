namespace Application.Features.VariantAttributes.Queries.GetVariantAttributePaginatedList;

public record GetVariantAttributePaginatedListResponse(Guid Id, string Name, string? Description, bool IsActive, DateTimeOffset CreatedTime);
