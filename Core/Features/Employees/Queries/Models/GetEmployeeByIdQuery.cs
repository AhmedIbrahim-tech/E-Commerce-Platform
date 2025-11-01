using Core.Features.Employees.Queries.Responses;

namespace Core.Features.Employees.Queries.Models
{
    public record GetEmployeeByIdQuery(Guid Id) : IRequest<ApiResponse<GetSingleEmployeeResponse>>;
}
