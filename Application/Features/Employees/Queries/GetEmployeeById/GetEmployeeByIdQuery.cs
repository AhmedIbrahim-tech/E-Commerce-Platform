using Application.Common.Bases;

namespace Application.Features.Employees.Queries.GetEmployeeById;

public record GetEmployeeByIdQuery(Guid Id) : IRequest<ApiResponse<GetEmployeeByIdResponse>>;

