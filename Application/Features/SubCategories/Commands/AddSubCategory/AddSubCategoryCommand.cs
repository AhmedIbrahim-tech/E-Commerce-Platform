using Application.Common.Bases;

namespace Application.Features.SubCategories.Commands.AddSubCategory;

public record AddSubCategoryCommand(string Name, string? Description, string? ImageUrl, string? Code, Guid CategoryId, bool IsActive) : IRequest<ApiResponse<string>>;
