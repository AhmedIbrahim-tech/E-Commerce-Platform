using Application.Common.Bases;

namespace Application.Features.Employees.Commands.DeleteEmployee;

public record DeleteEmployeeCommand(Guid Id) : IRequest<ApiResponse<string>>;

