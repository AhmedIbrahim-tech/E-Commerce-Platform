using Application.Common.Bases;

namespace Application.Features.VariantAttributes.Commands.DeleteVariantAttribute;

public record DeleteVariantAttributeCommand(Guid Id) : IRequest<ApiResponse<string>>;
