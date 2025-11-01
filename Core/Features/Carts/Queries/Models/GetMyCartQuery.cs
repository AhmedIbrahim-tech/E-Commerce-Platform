using Core.Features.Carts.Queries.Responses;

namespace Core.Features.Carts.Queries.Models
{
    public record GetMyCartQuery : IRequest<ApiResponse<GetSingleCartResponse>>;
}
