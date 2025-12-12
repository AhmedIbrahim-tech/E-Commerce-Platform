using Application.Common.Bases;

namespace Application.Features.Categories.Commands.AddCategory;

public record AddCategoryCommand(string Name, string? Description) : IRequest<ApiResponse<string>>;

