using Application.Common.Bases;

namespace Application.Features.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : IRequest<ApiResponse<string>>;

