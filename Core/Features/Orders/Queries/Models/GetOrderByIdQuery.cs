using Core.Features.Orders.Queries.Responses;

namespace Core.Features.Orders.Queries.Models;
public record GetOrderByIdQuery(Guid Id) : IRequest<ApiResponse<GetSingleOrderResponse>>
{
    public int OrderPageNumber { get; set; }
    public int OrderPageSize { get; set; }
};
