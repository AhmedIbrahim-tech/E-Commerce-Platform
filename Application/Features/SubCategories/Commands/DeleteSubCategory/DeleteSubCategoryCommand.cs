using Application.Common.Bases;

namespace Application.Features.SubCategories.Commands.DeleteSubCategory;

public record DeleteSubCategoryCommand(Guid Id) : IRequest<ApiResponse<string>>;
