using Core.Features.Carts.Queries.Responses;

namespace Core.Features.Carts.Queries.Models
{
    public record GetCartByIdQuery(Guid Id) : IRequest<ApiResponse<GetSingleCartResponse>>;
}
