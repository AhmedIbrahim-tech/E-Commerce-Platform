
namespace Core.Features.Categories.Commands.Models
{
    public record EditCategoryCommand(Guid Id, string Name, string? Description) : IRequest<ApiResponse<string>>;
}