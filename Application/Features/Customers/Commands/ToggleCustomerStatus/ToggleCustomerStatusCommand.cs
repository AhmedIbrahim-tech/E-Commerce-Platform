using Application.Common.Bases;
using MediatR;

namespace Application.Features.Customers.Commands.ToggleCustomerStatus;

public record ToggleCustomerStatusCommand(Guid Id) : IRequest<ApiResponse<string>>;
