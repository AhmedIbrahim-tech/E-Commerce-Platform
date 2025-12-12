using Application.Common.Bases;

namespace Application.Features.Customers.Commands.DeleteCustomer;

public record DeleteCustomerCommand(Guid Id) : IRequest<ApiResponse<string>>;

