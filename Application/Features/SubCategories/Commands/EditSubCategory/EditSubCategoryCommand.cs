using Application.Common.Bases;

namespace Application.Features.SubCategories.Commands.EditSubCategory;

public record EditSubCategoryCommand(Guid Id, string Name, string? Description, string? ImageUrl, string? Code, Guid CategoryId, bool IsActive) : IRequest<ApiResponse<string>>;
