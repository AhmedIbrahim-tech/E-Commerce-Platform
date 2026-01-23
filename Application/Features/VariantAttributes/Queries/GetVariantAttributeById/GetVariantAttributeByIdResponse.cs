namespace Application.Features.VariantAttributes.Queries.GetVariantAttributeById;

public record GetVariantAttributeByIdResponse(Guid Id, string Name, string? Description, bool IsActive, DateTimeOffset CreatedTime);
