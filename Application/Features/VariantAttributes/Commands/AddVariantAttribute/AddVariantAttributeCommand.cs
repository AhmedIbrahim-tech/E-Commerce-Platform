using Application.Common.Bases;

namespace Application.Features.VariantAttributes.Commands.AddVariantAttribute;

public record AddVariantAttributeCommand(string Name, string? Description, bool IsActive) : IRequest<ApiResponse<string>>;
