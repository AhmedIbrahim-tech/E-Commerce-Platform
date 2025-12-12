using Application.Common.Bases;

namespace Application.Features.Customers.Queries.GetCustomerById;

public record GetCustomerByIdQuery(Guid Id) : IRequest<ApiResponse<GetCustomerByIdResponse>>;

