
namespace Core.Features.Employees.Commands.Models
{
    public record DeleteEmployeeCommand(Guid Id) : IRequest<ApiResponse<string>>;
}
