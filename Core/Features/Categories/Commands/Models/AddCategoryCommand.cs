
namespace Core.Features.Categories.Commands.Models
{
    public record AddCategoryCommand(string Name, string? Description) : IRequest<ApiResponse<string>>;
}
