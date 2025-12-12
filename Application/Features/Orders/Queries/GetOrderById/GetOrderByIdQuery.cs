using Application.Common.Bases;

namespace Application.Features.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(Guid Id) : IRequest<ApiResponse<GetOrderByIdResponse>>
{
    public int OrderPageNumber { get; set; }
    public int OrderPageSize { get; set; }
}

