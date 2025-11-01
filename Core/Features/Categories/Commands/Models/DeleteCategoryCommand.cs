
namespace Core.Features.Categories.Commands.Models
{
    public record DeleteCategoryCommand(Guid Id) : IRequest<ApiResponse<string>>;
}
