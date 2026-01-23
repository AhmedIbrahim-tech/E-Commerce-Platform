namespace Application.Features.VariantAttributes.Queries.GetVariantAttributeList;

public record GetVariantAttributeListResponse(Guid Id, string Name, string? Description, bool IsActive, DateTimeOffset CreatedTime);
