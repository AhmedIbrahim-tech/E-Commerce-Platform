using Application.Common.Bases;

namespace Application.Features.Brands.Commands.DeleteBrand;

public record DeleteBrandCommand(Guid Id) : IRequest<ApiResponse<string>>;
