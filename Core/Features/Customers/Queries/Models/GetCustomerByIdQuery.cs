using Core.Features.Customers.Queries.Responses;

namespace Core.Features.Customers.Queries.Models
{
    public record GetCustomerByIdQuery(Guid Id) : IRequest<ApiResponse<GetSingleCustomerResponse>>;
}
