using Application.Common.Bases;

namespace Application.Features.VariantAttributes.Commands.EditVariantAttribute;

public record EditVariantAttributeCommand(Guid Id, string Name, string? Description, bool IsActive) : IRequest<ApiResponse<string>>;
