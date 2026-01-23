using Application.Common.Bases;

namespace Application.Features.Brands.Commands.AddBrand;

public record AddBrandCommand(string Name, string? Description, string? ImageUrl, bool IsActive) : IRequest<ApiResponse<string>>;
