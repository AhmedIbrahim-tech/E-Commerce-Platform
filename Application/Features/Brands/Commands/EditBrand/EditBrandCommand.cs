using Application.Common.Bases;

namespace Application.Features.Brands.Commands.EditBrand;

public record EditBrandCommand(Guid Id, string Name, string? Description, string? ImageUrl, bool IsActive) : IRequest<ApiResponse<string>>;
