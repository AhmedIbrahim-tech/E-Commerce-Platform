
namespace Core.Features.Customers.Commands.Models
{
    public record DeleteCustomerCommand(Guid Id) : IRequest<ApiResponse<string>>;
}
